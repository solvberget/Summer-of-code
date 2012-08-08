using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Lucene.Net.Index;
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




        private readonly string _pathToDictDir;
   
        private readonly string _pathToSuggestionsDict;

        private readonly IRepository _documentRepository;

        public LuceneRepository(string pathToDictionaryDirectory = null, string pathToSuggestionListDict = null, IRepository documentRepository = null)
        {

            _pathToSuggestionsDict = string.IsNullOrEmpty(pathToSuggestionListDict)
                ? @"App_Data\ordlister\ord_forslag.txt" : pathToSuggestionListDict;

            _pathToDictDir = string.IsNullOrEmpty(pathToDictionaryDirectory)
                ? @"App_Data\ordlister_index" : pathToDictionaryDirectory;

            _suggestionList = new HashMap();

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
                    }


                    if (document.SubTitle != null)
                    {
                        _suggestionList.Add(document.SubTitle);
                        try
                        {
                            DictionaryBuilder.Add(_pathToDictDir, document.SubTitle.ToLower());
                        }
                        catch (Exception e)
                        {
                            
                            Console.WriteLine(e.Message);
                        }
                     

                    }

                }


            }
            WriteSuggestionListToFile();
            //Task.Factory.StartNew(SuggestionListBuildDictionary);
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

        public string Lookup(string value)
        {
            InitializeSpellChecker();
            UpdateSuggestionListFromAlephSearch(value);

            // Escape harmful values in input string
            value = System.Security.SecurityElement.Escape(value);

            if (string.IsNullOrEmpty(value)) return string.Empty;

            var similarWords = SpellChecker.SuggestSimilar(value, 1);
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
