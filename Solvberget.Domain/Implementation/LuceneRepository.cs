using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Ninject;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using System.Collections.Generic;
using System.Linq;
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
        
        private const string PathToStopWordsDict = @"C:\Projects\Solvberget\Solvberget.Service\App_Data\ordlister\bokmal\stopwords.txt";

        public LuceneRepository(string pathToDictionary = null, string pathToDictionaryDirectory = null)
        {      
 
            _pathToDict = string.IsNullOrEmpty(pathToDictionary) 
                ? @"App_Data\ordlister\ord_test.txt" : pathToDictionary;

            _pathToDictDir = string.IsNullOrEmpty(pathToDictionaryDirectory) 
                ? @"App_Data\ordlister_index" : pathToDictionaryDirectory;
            
            InitializeSpellChecker();
            
        
        }

        private void InitializeSpellChecker()
        {
            var di = CreateTargetFolder();
            SpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di));
 StopWords =  File.ReadAllLines(PathToStopWordsDict);
        }

        private DirectoryInfo CreateTargetFolder()
        {
            var di = new DirectoryInfo(_pathToDictDir);
            if (!di.Exists)
            {
                Directory.CreateDirectory(_pathToDictDir);
            }
            return di;
        }

        public void BuildDictionary()
        {

            var di = CreateTargetFolder();
            var fi = new FileInfo(_pathToDict);

            using (var staticSpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di)))
            {
                staticSpellChecker.IndexDictionary(new PlainTextDictionary(fi));
            }

        }

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

        private bool IsStopWord(string value)
        {
            return StopWords.Contains(value);
        }

        public string Lookup(string value)
        {

            if (SpellChecker == null)
                InitializeSpellChecker();

            if (SpellChecker == null)
                return String.Empty;

            // Escape harmful values in input string
            value = System.Security.SecurityElement.Escape(value);

            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = WordSplitErrorMerger(value);

            var suggestionValue = string.Empty;

            foreach (var word in value.Split().Where(word => !string.IsNullOrEmpty(word)))
            {
                if (IsStopWord( word ))
                {

                    suggestionValue += word;
                    suggestionValue += " ";
                    continue;

                }
            
                // If the word exist in our dictionary, use it
                if( SpellChecker.Exist(word.ToLower()))
                {

                    suggestionValue += word;
                    suggestionValue += " ";
                    continue;

                }


                var wordSuggestions = SpellChecker.SuggestSimilar(word, 5);

                if ( wordSuggestions != null && wordSuggestions.Length > 0 )
                {
                    var wordSuggestion = wordSuggestions[0];
                    
                    // Preserver case, e.g Fotbal will convert to Fotball, not convert to fotball. 
                    if (char.IsUpper(word[0]))
                        wordSuggestion = char.ToUpper(wordSuggestion[0]) + wordSuggestion.Substring(1);

                    suggestionValue += wordSuggestion;
                }
                else
                    suggestionValue += word;

                suggestionValue += " ";
            }

            return Equals(suggestionValue[suggestionValue.Length - 1], ' ') ? suggestionValue.Substring(0, suggestionValue.Length - 1) : suggestionValue;
        }

    }


}
