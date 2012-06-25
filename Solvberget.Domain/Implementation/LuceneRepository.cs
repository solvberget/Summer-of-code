using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using System.Collections.Generic;
using System.Linq;
using SpellChecker.Net.Search.Spell;

namespace Solvberget.Domain.Implementation
{



    public class LuceneRepository : ISpellingDictionary
    {

        private SpellChecker.Net.Search.Spell.SpellChecker SpellChecker { get; set; }
        public string[] StopWords { get; set; }

        //TODO: Legge ut i config-fil
        private const string PathToDict =
            @"C:\Projects\Solvberget\Solvberget.Service\App_Data\ordlister\bokmal\ord_bm.txt";

        private const string PathToStopWordsDict = @"C:\Projects\Solvberget\Solvberget.Service\App_Data\ordlister\bokmal\stopwords.txt";

        private const string PathToDictDir = @"C:\Projects\Solvberget\Solvberget.Service\App_Data\ordlister\bokmal";

        /** INITIALIZATION **/

        public LuceneRepository()
        {

            InitializeSpellChecker();
        }

        private void InitializeSpellChecker()
        {

            var di = new DirectoryInfo(PathToDictDir);
            SpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di));
            StopWords =  File.ReadAllLines(PathToStopWordsDict);

        }

        /** STATICS **/

        public static void BuildDictionary()
        {

            var di = new DirectoryInfo(PathToDictDir);
            var fi = new FileInfo(PathToDict);

            using (var staticSpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di)))
            {
                staticSpellChecker.IndexDictionary(new PlainTextDictionary(fi));
            }

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

        /** MAIN FUNCTIONS */

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

            return Equals(suggestionString[suggestionString.Length - 1], ' ') ? suggestionString.Substring(0, suggestionString.Length - 1) : suggestionString;
        }

    }


}
