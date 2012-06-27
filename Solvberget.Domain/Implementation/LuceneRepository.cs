using System;
using System.IO;
using System.Linq;
using Lucene.Net.Store;
using Ninject;
using Solvberget.Domain.Abstract;
using SpellChecker.Net.Search.Spell;

using Directory = System.IO.Directory;

namespace Solvberget.Domain.Implementation
{


    public class LuceneRepository : ISpellingDictionary
    {

        private SpellChecker.Net.Search.Spell.SpellChecker SpellChecker { get; set; }
        public string[] StopWords { get; set; }


        //TODO: Legge ut i config-fil
        private readonly string _pathToDict;
        private readonly string _pathToDictDir;
        private readonly string _pathToStopwordsDict;
        private readonly string _pathToSuggestionsDict;
        private readonly string _pathToTestDict;

        private string[] _suggestionList;

        public LuceneRepository(string pathToDictionary = null, string pathToDictionaryDirectory = null, string pathToStopWordsDict = null, string pathToSuggestionListDict = null, string pathToTestDict=null)
        {
            _pathToStopwordsDict = string.IsNullOrEmpty(pathToStopWordsDict)
                ? @"App_Data\ordlister\stopwords.txt" : pathToStopWordsDict;

            _pathToDict = string.IsNullOrEmpty(pathToDictionary) 
                ? @"App_Data\ordlister\ord_bm.txt" : pathToDictionary;

            _pathToSuggestionsDict = string.IsNullOrEmpty(pathToSuggestionListDict)
                ? @"App_Data\ordlister\ord_forslag.txt" : pathToSuggestionListDict;

            _pathToDictDir = string.IsNullOrEmpty(pathToDictionaryDirectory) 
                ? @"App_Data\ordlister_index" : pathToDictionaryDirectory;

            _pathToTestDict = string.IsNullOrEmpty(pathToTestDict)
                ? @"App_Data\ordlister\ord_test.txt" : pathToTestDict;

            InitializeSpellChecker();
        }

        private void InitializeSpellChecker()
        {
            if (SpellChecker != null) return;

            var di = DictionaryBuilder.CreateTargetFolder(_pathToDictDir);
            SpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di));
            _suggestionList = File.ReadAllLines(_pathToSuggestionsDict);
            StopWords = File.ReadAllLines(_pathToStopwordsDict);
        }

    



        /** HELPERS **/

        private void ResetStrings(ref string prevWord, ref string currWord)
        {
            prevWord = string.Empty;
            currWord = string.Empty;
        }

        private void UpdateStringsToWord(ref string prevWord, ref string currWord, string word)
        {
            prevWord = word;
            currWord = word;
        }

        private void AddPrevAndCurrentWord( ref string wordErrorsRemovedString, ref string prevWord, string word )
        {
            wordErrorsRemovedString += prevWord + " " + word;
            wordErrorsRemovedString += " ";
        }

        private bool IsStopWord(string value)
        {
            return StopWords.Contains(value);
        }

        private void AddWordToSuggestionString(ref string suggestionString, string word)
        {
            suggestionString += word + " ";
        }

        private string WordSplitErrorMerger( string value )
        {

            var wordErrorsRemovedString = string.Empty;
            var prevWord = string.Empty;
            var currentWord = string.Empty;

            foreach (var word in value.Split())
            {

                if (string.IsNullOrEmpty(prevWord))
                {
                    UpdateStringsToWord( ref prevWord, ref currentWord, word);
                    continue;
                }

                if (IsStopWord(word))
                {
                    AddPrevAndCurrentWord(ref wordErrorsRemovedString, ref prevWord, word);
                    ResetStrings( ref prevWord, ref currentWord );
                    continue;
                }

                var combinedWord = prevWord + word;
                if (SpellChecker.Exist(combinedWord.ToLower()))
                {
                    wordErrorsRemovedString += combinedWord;
                    ResetStrings( ref prevWord, ref currentWord );
                }
                else
                {
                    wordErrorsRemovedString += prevWord;
                    UpdateStringsToWord(ref prevWord, ref currentWord, word);
                }

                wordErrorsRemovedString += " ";

            }
            return string.IsNullOrEmpty(wordErrorsRemovedString) ? prevWord : wordErrorsRemovedString + currentWord;

        }



        public string Lookup(string value)
        {

            InitializeSpellChecker();

            // Escape harmful values in input string
            value = System.Security.SecurityElement.Escape(value);

            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = WordSplitErrorMerger(value);

            var suggestionString = string.Empty;

            foreach (var word in value.Split().Where(word => !string.IsNullOrEmpty(word)))
            {
                if (IsStopWord( word ))
                {
                    AddWordToSuggestionString(ref suggestionString, word);
                    continue;
                }
            
                // If the word exist in our dictionary, use it
                if( SpellChecker.Exist(word.ToLower()))
                {
                    AddWordToSuggestionString(ref suggestionString, word);
                    continue;
                }

                var similarWords = SpellChecker.SuggestSimilar(word, 5);

                if ( similarWords != null && similarWords.Length > 0 )
                {
                    var suggestionWord = similarWords[0];
                    
                    // Preserver case, e.g Fotbal will convert to Fotball, not convert to fotball. 
                    if (char.IsUpper(word[0]))
                        suggestionWord = char.ToUpper(suggestionWord[0]) + suggestionWord.Substring(1);

                    AddWordToSuggestionString(ref suggestionString, suggestionWord);
                }
                else
                    AddWordToSuggestionString(ref suggestionString, word);

            }
            return suggestionString.TrimEnd();
        }

        public string[] SuggestionList()
        {
            var suggestions = _suggestionList ?? File.ReadAllLines(_pathToSuggestionsDict);
            return suggestions;
        }
    }


}
