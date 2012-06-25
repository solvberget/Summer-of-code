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

        private SpellChecker.Net.Search.Spell.SpellChecker SpellChecker { get; set; }
        //TODO: Legge ut i config-fil
        private const string PathToDict = @"C:\Projects\Solvberget\Solvberget.Service\App_Data\ordlister\bokmal\ord_bm.txt";
        private const string PathToDictDir = @"C:\Projects\Solvberget\Solvberget.Service\App_Data\ordlister\bokmal";

        public LuceneRepository()
        {      

            InitializeSpellChecker  ();
        }

        private void InitializeSpellChecker ()
        {
             var di = new DirectoryInfo(PathToDictDir);
             SpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di));
        }

        public static void BuildDictionary()
        {

            var di = new DirectoryInfo(PathToDictDir);
            var fi = new FileInfo(PathToDict);

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
