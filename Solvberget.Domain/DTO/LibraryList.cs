using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solvberget.Domain.DTO
{
    public class LibraryList
    {
        public string Name { get; set; }
        public IEnumerable<string> DocumentNumbers;
    }
}
