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
        public string ExplanatoryAddition { get; set; }

        public string SheetmusicPublisher { get; set; }

        public string NumberOfPagesTypeAndNumberOfVotes { get; set; }

        public string DiscContent { get; set; }
        public string Type { get; set; }
        public string Genre { get; set; }
        public string RefferedPersons { get; set; }
        public  IEnumerable<Person> InvolvedPersons { get; set; }
       
        public string LivingYears { get; set; }
        public string Role { get; set; }
        public string Nationality { get; set; }
        public string SongTitles { get; set; }
        public string Group { get; set; }
        public string TitlesOtherWritingForms { get; set; }

        protected override void FillProperties(string xml)
        {


            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants("oai_marc");

                FillPropertiesLight(xml);

                StandardizedTitle = GetVarfield(nodes, "240", "a");
                ExplanatoryAddition = GetVarfield(nodes, "245", "a");

                SheetmusicPublisher = GetVarfield(nodes, "260", "b");

                NumberOfPagesTypeAndNumberOfVotes = GetVarfield(nodes, "300", "a");

                DiscContent = GetVarfield(nodes, "505", "a");
                Type = GetVarfield(nodes, "652", "a");
                Genre = GetVarfield(nodes, "655", "a");
                RefferedPersons = GetVarfield(nodes, "658", "a");
                InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                LivingYears = GetVarfield(nodes, "700", "d");
                Role = GetVarfield(nodes, "700", "e");
                Nationality = GetVarfield(nodes, "700", "j");
                SongTitles = GetVarfield(nodes, "700", "t");
                Group = GetVarfield(nodes, "710", "a");
                TitlesOtherWritingForms = GetVarfield(nodes, "740", "a");
            }
        }

        protected override void FillPropertiesLight(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants();
               
                 //Author, check BSMARC field 100 for author
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

            }
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