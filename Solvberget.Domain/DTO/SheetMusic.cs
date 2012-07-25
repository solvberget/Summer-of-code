using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class SheetMusic : Document
    {
        public Person Composer { get; set; }
        public string StandardizedTitle { get; set; }
        public string NumberOfPagesAndNumberOfParts { get; set; }
        public string DiscContent { get; set; }
        public string CompositionType { get; set; }
        public string Genre { get; set; }
        public IEnumerable<string> MusicalLineup { get; set; }
        public IEnumerable<Person> InvolvedPersons { get; set; }
        public string TitlesOtherWritingForms { get; set; }

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                StandardizedTitle = GetVarfield(nodes, "240", "a");
                NumberOfPagesAndNumberOfParts = GetVarfield(nodes, "300", "a");
                DiscContent = GetVarfield(nodes, "505", "a");
                CompositionType = GetVarfield(nodes, "652", "a");
                Genre = GetVarfield(nodes, "655", "a");
                MusicalLineup = GetVarfieldAsList(nodes, "658", "a");
                InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                TitlesOtherWritingForms = GetVarfield(nodes, "740", "a");
               
            }
        }

        protected override void FillPropertiesLight(string xml)
        {
            base.FillPropertiesLight(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants();
               
                //Composer
                var nationality = GetVarfield(nodes, "100", "j");
                string nationalityLookupValue = null;
                if (nationality != null)
                    NationalityDictionary.TryGetValue(nationality, out nationalityLookupValue);
                Composer =  new Person
                {
                    
                    Name = GetVarfield(nodes, "100", "a"),
                    LivingYears = GetVarfield(nodes, "100", "d"),
                    Nationality = nationalityLookupValue ?? nationality,
                    Role = "Composer"
                };
                string tempName = GetVarfield(nodes, "100", "a");
                if (tempName != null)
                    Composer.SetName(tempName);

            }
            
        }

        public override string GetCompressedString()
        {
            string docTypeLookupValue = null;
            if (DocType != null)
            {
                DocumentDictionary.TryGetValue(DocType, out docTypeLookupValue);
            }

            var temp = docTypeLookupValue ?? DocType;
            if (Composer.Name != null)
            {
                temp += ", " + Composer.Name;
            }
            if (PublishedYear != 0)
            {
                temp += " (" + PublishedYear + ")";
            }
            return temp;


        }

        public new static SheetMusic GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var document = new SheetMusic();

            document.FillProperties(xml);

            return document;
        }

        public new static SheetMusic GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var document = new SheetMusic();

            document.FillPropertiesLight(xml);

            return document; ;
        }

    }
}