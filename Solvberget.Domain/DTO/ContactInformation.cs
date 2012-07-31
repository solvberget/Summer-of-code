using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Solvberget.Domain.DTO
{
    public sealed class ContactInformation : Information
    {
        // Same property names as XML-doc
        // ReSharper disable InconsistentNaming
        public string Title { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        public string PhoneOpeningHours { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string VisitorAddress { get; set; }
        public Uri MapLink { get; set; }
        public IEnumerable<Person> ContactPersons { get; set; }


       

    }
}
