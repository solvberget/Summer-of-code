using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Solvberget.Domain.Implementation
{
    


    public class LuceneRepository : ISpellingDictionary
    {

        private void buildDictionary()
        {
            // TODO: Hent ordliste, parse, 

            
        }

        public List<String> Lookup(string value)
        {
            Stream s = new StreamReader("file.txt",Encoding.UTF8).BaseStream.;

            IndexReader.Open("file.txt");

            return null;
        }
    }

}
