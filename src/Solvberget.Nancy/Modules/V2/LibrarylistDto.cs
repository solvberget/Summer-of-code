using System.Collections.Generic;

namespace Solvberget.Nancy.Modules.V2
{
    public class LibrarylistDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DocumentDto> Documents { get; set; }
    }
}