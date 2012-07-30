using System;
using System.Collections.Generic;

namespace Solvberget.Domain.DTO
{
    public sealed class ContactInformation
    {
        // Same property names as XML-doc
        // ReSharper disable InconsistentNaming
        public string Department { get; set; }
        public string Phone { get; set; }
        public string PhoneOpeningHours { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string VisitorAddress { get; set; }
        public Uri MapLink { get; set; }
        public IEnumerable<Person> ContactPersons { get; set; }
        public string OtherInformation { get; set; }
        // ReSharper restore InconsistentNaming

    }
}
