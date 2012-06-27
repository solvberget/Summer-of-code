using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Store;
using SpellChecker.Net.Search.Spell;
using Directory = System.IO.Directory;

namespace Solvberget.Domain.Implementation
{
    public class DictionaryBuilder
    {
        public static void Build(string dictionaryPath, string indexPath)
        {

            var di = CreateTargetFolder(indexPath);
            // var fi = new FileInfo(_pathToDict);
            var fi = new FileInfo(dictionaryPath);
            using (var staticSpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di)))
            {
                staticSpellChecker.IndexDictionary(new PlainTextDictionary(fi));
            }

        }

        public static DirectoryInfo CreateTargetFolder(string path)
        {
            var di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                Directory.CreateDirectory(path);
            }
            return di;
        }
    }
}
