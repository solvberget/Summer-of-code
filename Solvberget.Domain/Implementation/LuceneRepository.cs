using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
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

        public SpellChecker.Net.Search.Spell.SpellChecker _SpellChecker { get; set; }

        private void buildDictionary()
        {
            const string path = @"C:\Projects\Solvberget\Solvberget.Service\App_Data\ordlister\bokmal\ord_bm.txt";
            const string path_dir = @"C:\Projects\Solvberget\Solvberget.Service\App_Data\ordlister\bokmal";
   
            var di = new DirectoryInfo(path_dir);
            var fi = new FileInfo(path);
     
            _SpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di));
            _SpellChecker.IndexDictionary(new PlainTextDictionary(fi));

        }

        public string[] Lookup(string value)
        {
            // TODO: FIX
            buildDictionary();
            string[] liste = _SpellChecker.SuggestSimilar(value, 5);
            return liste;
         
        }
    }

}
