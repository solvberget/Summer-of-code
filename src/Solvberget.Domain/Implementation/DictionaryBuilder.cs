using System;
using System.IO;
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
            using (var file = File.Open(dictionaryPath, FileMode.Open, FileAccess.Read))
            {
                var dict = new PlainTextDictionary(file);
                using (var staticSpellChecker = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(di)))
                {
                    try
                    {
                        staticSpellChecker.IndexDictionary(dict);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
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
