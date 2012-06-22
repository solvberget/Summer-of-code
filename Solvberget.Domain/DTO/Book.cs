using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Book : Document
    {
        public string Author { get; set; }

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                
                //Get root oai_marc XML-element
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                
                //Get author
                //Check BSMARC field 100 for author
                var mainSchemeWord = nodes.Elements("varfield").Where(x => ((string) x.Attribute("id")).Equals("100")).Elements("subfield");

                //If N/A, check BSMARC field 110 for author
                if (mainSchemeWord == null)
                    mainSchemeWord = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("110")).Elements("subfield");
                
                //If still N/A, check BSMARC field 130 for title when title is main scheme word
                if (mainSchemeWord == null)
                    mainSchemeWord = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("130")).Elements("subfield");

                //Set the obtained word
                if(mainSchemeWord != null)
                    Author = mainSchemeWord.Where(x => ((string) x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();
                
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