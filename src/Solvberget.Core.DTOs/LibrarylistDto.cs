using System.Collections.Generic;

namespace Solvberget.Core.DTOs
{
    public class LibrarylistDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public List<DocumentDto> Documents { get; set; }
    }
}