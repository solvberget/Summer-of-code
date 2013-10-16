using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;
using SpellChecker.Net.Search.Spell;

using Directory = System.IO.Directory;

namespace Solvberget.Domain.Implementation
{


    public class LuceneRepository : ISuggestionDictionary
    {


        private SpellChecker.Net.Search.Spell.SpellChecker SpellChecker { get; set; }

        private readonly string _pathToDictDir;

        private readonly string _pathToSuggestionsDict;

        private readonly IRepository _documentRepository;

        public LuceneRepository(IEnvironmentPathProvider environment, IRepository documentRepository = null)
        {
            var indexPath = environment.GetDictionaryIndexPath();
            var suggestionPath = environment.GetSuggestionListPath();

            _pathToSuggestionsDict = string.IsNullOrEmpty(suggestionPath)
                ? @"App_Data\ordlister\ord_forslag.txt" : suggestionPath;
            
            if (!File.Exists(_pathToSuggestionsDict)) File.Create(_pathToSuggestionsDict); 
            
            _pathToDictDir = string.IsNullOrEmpty(indexPath)
                ? @"App_Data\ordlister_index" : indexPath;

            _suggestionList = new HashSet<string>();

            _documentRepository = documentRepository;


        }

        private void InitializeSpellChecker()
        {
            if (SpellChecker != null) return;

            var di = DictionaryBuilder.CreateTargetFolder(_pathToDictDir);
            SpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di));
            InitSuggestionListFromFile();
        }


        /**
         * SUGGESTION LIST
         **/

        private readonly HashSet<string> _suggestionList;

        public void UpdateSuggestionListFromAlephSearch(string searchValue)
        {

            InitSuggestionListFromFile();
            var documentList = _documentRepository.Search(searchValue);
            if (documentList.Count > 1)
            {
                foreach (var document in documentList)
                {

                    if (document.Title != null)
                    {
                        _suggestionList.Add(document.Title);
                    }


                    if (document.SubTitle != null)
                    {
                        _suggestionList.Add(document.SubTitle);
                    }

                }


            }
            WriteSuggestionListToFile();


        }



        private void WriteSuggestionListToFile()
        {
            try
            {
                File.WriteAllLines(_pathToSuggestionsDict, _suggestionList.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine("Kan ikke lagre suggestion: " + e.Message);
            }

        }



        private void InitSuggestionListFromFile()
        {
            var tempList = File.ReadAllLines(_pathToSuggestionsDict);

            foreach (var word in tempList)
            {
                _suggestionList.Add(word);
            }
        }


        /** HELPERS **/
        Encoding iso = Encoding.GetEncoding("ISO-8859-1");
        Encoding utf8 = Encoding.UTF8;
        public string Lookup(string value)
        {
            InitializeSpellChecker();
            UpdateSuggestionListFromAlephSearch(value);

            // Escape harmful values in input string
            value = System.Security.SecurityElement.Escape(value);

            if (string.IsNullOrEmpty(value)) return string.Empty;

            var similarWords = SpellChecker.SuggestSimilar(value, 1);

            if (similarWords.Any())
            {
                var upperCaseFirst = UppercaseFirst(similarWords[0]);
                var lowerCaseFirst =LowerCaseFirst(similarWords[0]);
                if (value.Equals(lowerCaseFirst)||value.Equals(upperCaseFirst) || value.Equals(similarWords[0].ToLower()))
                    return "";
                return utf8.GetString(iso.GetBytes(similarWords[0]));

            }
            return "";

        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        static string LowerCaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToLower(s[0]) + s.Substring(1);
        }

        public string[] SuggestionList()
        {
            InitSuggestionListFromFile();
            var suggestions = _suggestionList.ToArray();
            return suggestions;
        }
    }


}
