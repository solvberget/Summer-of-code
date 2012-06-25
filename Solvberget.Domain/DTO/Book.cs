using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Book : Document
    {

        public string Isbn { get; set; }
        public string LanguagesInBook { get; set; }
        public string ClassificationNr { get; set; }
        public string Author { get; set; }
        public string Organization { get; set; }
        public string UnderOrganization { get; set; }
        public string OrganizationAddedExplination { get; set; }
        public string StandarizedTitle { get; set; }
        public string StdOrOrgTitle { get; set; }
        public string Numbering { get; set; }
        public string PartTitle { get; set; }
        public string Edition { get; set; }
        public string NumberOfPages { get; set; }
        public string Content { get; set; }

        public IEnumerable<string> ReferencedPlaces { get; set; } 
        public IEnumerable<string> Subject { get; set; }
        public IEnumerable<string> Genre { get; set; }

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                
                //Get root oai_marc XML-element
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                
                //Get ISBN (020a)
                Isbn = GetVarfield(nodes, "020", "a");

                //TODO: Tester
                //Only get languages (041a) if language in base equals "mul" 
                if (Language.Equals("mul"))
                {
                    //var langsInBook = GetVarfield(nodes, "041", "a");
                    //LanguagesInBook = langsInBook.SplitByLength(3).ToList();
                }

                //Classification number (090c)
                ClassificationNr = GetVarfield(nodes, "090", "c");

                //TODO: Tester for 110 og 130
                //Author
                //Check BSMARC field 100 for author
                Author = GetVarfield(nodes, "100", "a");

                //If N/A, check BSMARC field 110 for author
                if (Author == null)
                    Author = GetVarfield(nodes, "110", "a");

                //If still N/A, check BSMARC field 130 for title when title is main scheme word
                if (Author == null)
                    Author = GetVarfield(nodes, "130", "a");

                //TODO: Tester
                //Organization (110abq)
                Organization = GetVarfield(nodes, "110", "a");
                UnderOrganization = GetVarfield(nodes, "110", "b");
                OrganizationAddedExplination = GetVarfield(nodes, "110", "q");

                //TODO: Test
                //Standarized title (130a)
                StandarizedTitle = GetVarfield(nodes, "130", "a");

                //TODO: Test
                //Standarized or orginal title (240a)
                StdOrOrgTitle = GetVarfield(nodes, "240", "a");

                //Numbering (245n)
                Numbering = GetVarfield(nodes, "245", "n");

                //TODO: Test
                //Title for part (245p)
                PartTitle = GetVarfield(nodes, "245", "n");
                
                //Edition (250a)
                Edition = GetVarfield(nodes, "250", "a");

                //Number of pages (300a)
                NumberOfPages = GetVarfield(nodes, "300", "a");

                //Content (505a)
                Content = GetVarfield(nodes, "505", "a");

                //Referenced places (651a)
                ReferencedPlaces = GetVarfieldAsList(nodes, "651", "a");

                //Subject (650a)
                Subject = GetVarfieldAsList(nodes, "650", "a");

                //Genre (655a)
                Genre = GetVarfieldAsList(nodes, "655", "a");

            }
        }

        public static Book GetBookFromFindDocXml(string xml)
        {
            var book = new Book();

            book.FillProperties(xml);

            return book;
        }
    }
}