using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Book : Document
    {

        public string Author { get; set; }
        public string SubTitle { get; set; }
        public IEnumerable<string> InvolvedPersons { get; set; } 

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");

                var TitleAndResponsebility = nodes.Elements("varfield")
                    .Where(x => ((string)x.Attribute("id")).Equals("245")).Elements("subfield");

                if (TitleAndResponsebility != null)
                {
                    Title = TitleAndResponsebility.Where(x => ((string)x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();
                    SubTitle = TitleAndResponsebility.Where(x => ((string) x.Attribute("label")).Equals("b")).Select(x => x.Value).FirstOrDefault();
                    InvolvedPersons = TitleAndResponsebility.Where(x => ((string) x.Attribute("label")).Equals("c")).Select(x => x.Value).FirstOrDefault().Split(';');
                }

                var MainSchemeWord = nodes.Elements("varfield").Where(x => ((string) x.Attribute("id")).Equals("100")).Elements("subfield");
                if (MainSchemeWord == null) MainSchemeWord = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("110")).Elements("subfield");

                if(MainSchemeWord != null)
                {
                    Author = MainSchemeWord.Where(x => ((string) x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();
                }
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