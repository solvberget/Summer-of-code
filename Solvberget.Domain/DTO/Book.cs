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
      //public string Type { get; set; } ClassificationNr??
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

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                
                //Get root oai_marc XML-element
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                
                //Get ISBN (020a)
                var isbn = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("020")).Elements("subfield");
                Isbn = isbn.Where(x => ((string) x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();

                //Only get languages (041a) if language in base equals "mul" 
                if (Language.Equals("mul"))
                {
                    var langsInBook = nodes.Elements("varfield").Where(x => ((string) x.Attribute("id")).Equals("041")).Elements("subfield");
                    LanguagesInBook = langsInBook.Where(x => ((string) x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();
                }

                //Get author
                //Check BSMARC field 100 for author
                var mainSchemeWord = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("100")).Elements("subfield");

                //If N/A, check BSMARC field 110 for author
                if (mainSchemeWord == null)
                    mainSchemeWord = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("110")).Elements("subfield");

                //If still N/A, check BSMARC field 130 for title when title is main scheme word
                if (mainSchemeWord == null)
                    mainSchemeWord = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("130")).Elements("subfield");

                //Set author if obtained word
                if (mainSchemeWord != null)
                    Author = mainSchemeWord.Where(x => ((string)x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();

                //Organization (110abq)
                var orgInfo = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("110")).Elements("subfield");
                Organization = orgInfo.Where(x => ((string)x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();
                UnderOrganization = orgInfo.Where(x => ((string)x.Attribute("label")).Equals("b")).Select(x => x.Value).FirstOrDefault();
                OrganizationAddedExplination = orgInfo.Where(x => ((string)x.Attribute("label")).Equals("q")).Select(x => x.Value).FirstOrDefault();

                //Standarized title
                var stdTitle = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("130")).Elements("subfield");
                StandarizedTitle = stdTitle.Where(x => ((string)x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();

                //Standarized or orginal title (240a)
                var stdOrOrgTitle = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("240")).Elements("subfield");
                StdOrOrgTitle = stdOrOrgTitle.Where(x => ((string)x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();

                //Numbering (245n)
                var numb = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("245")).Elements("subfield");
                Numbering = numb.Where(x => ((string)x.Attribute("label")).Equals("n")).Select(x => x.Value).FirstOrDefault();

                //Title for part (245p)
                var partTitle = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("245")).Elements("subfield");
                PartTitle = partTitle.Where(x => ((string)x.Attribute("label")).Equals("n")).Select(x => x.Value).FirstOrDefault();

                //Edition (250a)
                var edition = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("250")).Elements("subfield");
                Edition = edition.Where(x => ((string)x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();


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