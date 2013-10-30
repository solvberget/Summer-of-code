using System.Collections.Generic;
using System.Xml.Linq;

namespace Solvberget.Domain.Documents
{
    public class Journal : Document
    {
        public new int StandardLoanTime { get { return 11; } }
        public string Issn { get; set; }
        public string JournalsPerYear { get; set; }
        public string InventoryInfomation { get; set; }
        public IEnumerable<string> Subject { get; set; }
        public IEnumerable<string> ReferencedPlaces { get; set; }
        public IEnumerable<Person> InvolvedPersons { get; set; }
        public IEnumerable<Organization> InvolvedOrganizations { get; set; }
        public IEnumerable<string> OtherTitles { get; set; }

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                JournalsPerYear = GetVarfield(nodes, "310", "a");
                InventoryInfomation = GetVarfield(nodes, "590", "a");
                Subject = GetVarfieldAsList(nodes, "650", "a");
                ReferencedPlaces = GetVarfieldAsList(nodes, "651", "a");
                InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                InvolvedOrganizations = GenerateOrganizationsFromXml(nodes, "710");
                OtherTitles = GetVarfieldAsList(nodes, "740", "a");
            }

        }

       

        protected override void FillPropertiesLight(string xml)
        {

            base.FillPropertiesLight(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                Issn = GetVarfield(nodes, "022", "a");
                InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                MainResponsible = Publisher;
            }
            
        }


        public new static Journal GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var domainObject = new Journal();

            domainObject.FillPropertiesLight(xml);

            return domainObject;
        }

        public new static Journal GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var domainObject = new Journal();

            domainObject.FillProperties(xml);

            return domainObject;
        }

    }
}