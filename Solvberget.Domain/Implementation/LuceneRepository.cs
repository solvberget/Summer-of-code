using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
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
        //TODO: Legge ut i config-fil
        private readonly string _pathToDict;
        private readonly string _pathToDictDir;

     
        public LuceneRepository(string pathToDictionary = null, string pathToDictionaryDirectory = null)
        {      
 
            _pathToDict = string.IsNullOrEmpty(pathToDictionary) 
                ? @"App_Data\ordlister\ord_test.txt" : pathToDictionary;

            _pathToDictDir = string.IsNullOrEmpty(pathToDictionaryDirectory) 
                ? @"App_Data\ordlister_index" : pathToDictionaryDirectory;
            
            InitializeSpellChecker();
            
        
        }

        private void InitializeSpellChecker ()
        {
            var di = CreateTargetFolder();
            SpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di));
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

        public string[] Lookup(string value)
        {
            if( SpellChecker == null )
            {
                InitializeSpellChecker();
            }

            if (SpellChecker != null)
            {
                var suggestions = SpellChecker.SuggestSimilar(value, 5);

                if (suggestions == null) throw new ArgumentNullException("value");
                return suggestions;
            }
            throw new ArgumentNullException("value");
        }
    }

}
