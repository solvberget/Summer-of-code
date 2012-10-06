using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public sealed class ContactInformation : Information
    {
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string VisitingAddress { get; set; }
        public IList<ContactPerson> ContactPersons { get; set; }
        public IList<string> GenericFields { get; set; }

        public static ContactInformation GenerateFromXml(XElement xml)
        {
            var ci = new ContactInformation();
            ci.FillProperties(xml);
            return ci;
        }

        private void FillProperties(XContainer xml)
        {
            if (xml == null) return;

            var title = xml.Element("title");
            var phone = xml.Element("phone");
            var fax = xml.Element("fax");
            var email = xml.Element("email");
            var visitingAddress = xml.Element("visitingaddress");
            var address = xml.Element("address");

            if (title != null) Title = string.IsNullOrEmpty(title.Value) ? null : title.Value;
            if (phone != null) Phone = string.IsNullOrEmpty(phone.Value) ? null : phone.Value;
            if (fax != null) Fax = string.IsNullOrEmpty(fax.Value) ? null : fax.Value;
            if (email != null) Email = string.IsNullOrEmpty(email.Value) ? null : email.Value;
            if (visitingAddress != null) VisitingAddress = string.IsNullOrEmpty(visitingAddress.Value) ? null : visitingAddress.Value;
            if (address != null) Address = string.IsNullOrEmpty(address.Value) ? null : address.Value;

            var contactPersons = xml.Element("contact-persons");
            
            if (contactPersons != null)
            {
                
                ContactPersons = new List<ContactPerson>();

                foreach (var person in contactPersons.Descendants("person"))
                {
                    var name = person.Element("name");
                    if (name == null) continue;
                    if (string.IsNullOrEmpty(name.Value)) continue;
                    var cp = new ContactPerson {Name = name.Value};
                
                    var positionAsElement = person.Element("position");
                    var emailAsElement = person.Element("email");
                    var phoneAsElement = person.Element("phone");

                    if (positionAsElement != null && !string.IsNullOrEmpty(positionAsElement.Value))
                        cp.Position = positionAsElement.Value;
                    if (emailAsElement != null && !string.IsNullOrEmpty(emailAsElement.Value))
                        cp.Email = emailAsElement.Value;
                    if (phoneAsElement != null && !string.IsNullOrEmpty(phoneAsElement.Value))
                        cp.Phone = phoneAsElement.Value;

                    ContactPersons.Add(cp);

                }
            }

            var genericFields = xml.Element("generic-fields");
            if (genericFields == null) return;
            GenericFields = new List<string>();
            var fields = genericFields.Descendants("field");
            foreach (var field in fields.Where(field => field != null && !string.IsNullOrEmpty(field.Value))){
                GenericFields.Add(field.Value);
            }

        }
    }
}
