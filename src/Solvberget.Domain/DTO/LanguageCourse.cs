using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class LanguageCourse : Document
    {
        public new int StandardLoanTime { get { return 60; } }
        public string Isbn { get; set; }
        public string ClassificationNr { get; set; }
        public Person Author { get; set; }
        public string TypeAndNumberOfDiscs { get; set; }
        public IEnumerable<string> Subject { get; set; }
        public IEnumerable<Person> InvolvedPersons { get; set; }
        public IEnumerable<Organization> InvolvedOrganizations { get; set; }
        public string TitlesOtherWritingForms { get; set; }
      
        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                FillPropertiesLight(xml);
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                Isbn = GetVarfield(nodes, "020", "a");
                ClassificationNr = GetVarfield(nodes, "090", "c");
                TypeAndNumberOfDiscs = GetVarfield(nodes, "300", "a");
                Subject = GetVarfieldAsList(nodes, "650", "a");
                InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                InvolvedOrganizations = GenerateOrganizationsFromXml(nodes, "710");
                TitlesOtherWritingForms = GetVarfield(nodes, "740", "a");
               
            }
        }

        protected override void FillPropertiesLight(string xml)
        {
            base.FillPropertiesLight(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                var nationality = GetVarfield(nodes, "100", "j");
                string nationalityLookupValue = null;
                if (nationality != null)
                    NationalityDictionary.TryGetValue(nationality, out nationalityLookupValue);
                Author = new Person
                {
                    Name = GetVarfield(nodes, "100", "a"),
                    LivingYears = GetVarfield(nodes, "100", "d"),
                    Nationality = nationalityLookupValue ?? nationality,
                    Role = "Author"

                };
                string tempName = GetVarfield(nodes, "100", "a");
                if (tempName != null)
                    Author.InvertName(tempName);
                //If N/A, check BSMARC field 110 for author
                if (Author.Name == null)
                { 
                   Author.Name = GetVarfield(nodes, "110", "a");
                   

                }
            }
        
        }


        protected override string GetCompressedString()
        {
            string docTypeLookupValue = null;
            if (DocType != null)
            {
                DocumentDictionary.TryGetValue(DocType, out docTypeLookupValue);
            }

            var temp = docTypeLookupValue ?? DocType;
            if (Author.Name != null)
            {
                temp += ", " + Author.Name;
            }
            if (PublishedYear != 0)
            {
                temp += " (" + PublishedYear + ")";
            }
            return temp;


        }

        public new static LanguageCourse GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var document = new LanguageCourse();

            document.FillProperties(xml);

            return document;
        }

        public new static LanguageCourse GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var document = new LanguageCourse();

            document.FillProperties(xml);

            return document;
        }

    }
}