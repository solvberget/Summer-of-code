using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Lucene.Net.Store;
using Ninject;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;
using SpellChecker.Net.Search.Spell;

using Directory = System.IO.Directory;

namespace Solvberget.Domain.Implementation
{


    public class LuceneRepository : ISpellingDictionary
    {

        private SpellChecker.Net.Search.Spell.SpellChecker SpellChecker { get; set; }
        private string[] StopWords { get; set; }



        private readonly string _pathToDictDir;
        private readonly string _pathToStopwordsDict;
        private readonly string _pathToSuggestionsDict;

        private readonly IRepository _documentRepository;

        public LuceneRepository(string pathToDictionary = null, string pathToDictionaryDirectory = null, string pathToStopWordsDict = null, string pathToSuggestionListDict = null, string pathToTestDict = null, IRepository documentRepository = null)
        {
            _pathToStopwordsDict = string.IsNullOrEmpty(pathToStopWordsDict)
                ? @"App_Data\ordlister\stopwords.txt" : pathToStopWordsDict;

            _pathToSuggestionsDict = string.IsNullOrEmpty(pathToSuggestionListDict)
                ? @"App_Data\ordlister\ord_forslag.txt" : pathToSuggestionListDict;

            _pathToDictDir = string.IsNullOrEmpty(pathToDictionaryDirectory)
                ? @"App_Data\ordlister_index" : pathToDictionaryDirectory;

            _suggestionList = new HashMap();

            _documentRepository = documentRepository;


        }



        public void SuggestionListBuildDictionary()
        {
            DictionaryBuilder.Build(_pathToSuggestionsDict, _pathToDictDir);
        }


        private void InitializeSpellChecker()
        {
            if (SpellChecker != null) return;

            var di = DictionaryBuilder.CreateTargetFolder(_pathToDictDir);
            SpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di));
            StopWords = File.ReadAllLines(_pathToStopwordsDict);
            InitSuggestionListFromFile();

        }


        /**
         * SUGGESTION LIST
         **/

        private readonly HashMap _suggestionList;

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

                        DictionaryBuilder.Add(_pathToDictDir, document.Title.ToLower());

                        //foreach (var word in document.Title.Split(' '))
                        //{
                        //   
                        //    DictionaryBuilder.Add(_pathToDictDir, word);
                        //    DictionaryBuilder.Add(_pathToDictDir, word.ToLower());
                        //}
                    }


                    if (document.SubTitle != null)
                    {
                        _suggestionList.Add(document.SubTitle);

                        DictionaryBuilder.Add(_pathToDictDir, document.SubTitle.ToLower());

                    }

                }


            }
            WriteSuggestionListToFile();
            Task.Factory.StartNew(SuggestionListBuildDictionary);

        }




        private void WriteSuggestionListToFile()
        {
            try
            {
                File.WriteAllLines(_pathToSuggestionsDict, _suggestionList.Values.ToArray());
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

        private void AddWordToSuggestionString(ref string suggestionString, string word)
        {
            suggestionString += word + " ";
        }

        public string Lookup(string value)
        {
            InitializeSpellChecker();
            UpdateSuggestionListFromAlephSearch(value);
            var documentList = _documentRepository.Search(value);

            // Escape harmful values in input string
            value = System.Security.SecurityElement.Escape(value);

            if (string.IsNullOrEmpty(value)) return string.Empty;



            var suggestionString = string.Empty;

            return GetSuggestionString(value, suggestionString).TrimEnd();

        }

        private string GetSuggestionString(string value, string suggestionString)
        {

            foreach (var word in value.Split().Where(word => !string.IsNullOrEmpty(word)))
            {
                if (!StopWords.Contains(word))
                {
                    AddWordToSuggestionString(ref suggestionString, word);
                    continue;
                }
            }

            var similarWords = SpellChecker.SuggestSimilar(suggestionString, 5);
            return similarWords.Any() ? similarWords[0] : "";
            
        }

        public string[] SuggestionList()
        {
            InitSuggestionListFromFile();
            var suggestions = _suggestionList.Values.ToArray();
            return suggestions;
        }
    }


}
