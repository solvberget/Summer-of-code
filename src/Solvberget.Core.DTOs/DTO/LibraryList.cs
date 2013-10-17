using System.Collections.Generic;

namespace Solvberget.Domain.DTO
{
    public class LibraryList
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public bool IsRanked { get; set; }
        public Dictionary<string, bool> DocumentNumbers { get; set; }
        public List<Document> Documents { get; set; } 

        public LibraryList()
        {
            DocumentNumbers = new Dictionary<string, bool>();
            Documents = new List<Document>();
        }
    }
}
