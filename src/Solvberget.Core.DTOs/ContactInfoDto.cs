using System.Collections.Generic;

namespace Solvberget.Core.DTOs
{
    public class ContactInfoDto
    {
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string VisitingAddress { get; set; }
        public IEnumerable<ContactPersonDto> ContactPersons { get; set; }
        public IEnumerable<string> GenericFields { get; set; }
    }
}
