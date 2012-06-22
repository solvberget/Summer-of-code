using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Solvberget.Domain.Implementation
{

    namespace MvcLuceneSampleApp.Search
    {
        public class SampleData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }



    namespace MvcLuceneSampleApp.Search {
        public static class SampleDataRepository {

            public static SampleData Get(int id) {

                return GetAll().SingleOrDefault(x => x.Id.Equals(id));

            }
            public static List<SampleData> GetAll() {

                return new List<SampleData> {

                    new SampleData {Id = 1, Name = "sommer", Description = "Ord for herligste høytiden"},
                    new SampleData {Id = 2, Name = "super", Description = "adjektiv"},
                    new SampleData {Id = 3, Name = "sommeren", Description = "Bestemt form"},
                    new SampleData {Id = 4, Name = "datamaskin", Description = "kodeobjekt pc"},
                    new SampleData {Id = 5, Name = "film", Description = "Noe man ser på"},
              
                };

            }
        }
    }

    public class LuceneRespository : ISpellingDictionary
    {

        public List<Document> Lookup(string value)
        {
            return null;
        }
    }

}
