using System;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Document
    {
        public string Title { get; set; }
        public string Language { get; set; }
        public string TargetGroup { get; set; }
        public string DocumentType { get; set; }
        public string PlacePublished { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Scope { get; set; }
        public string Category { get; set; }
        public string AlphabeticalStatement { get; set; }

        protected virtual void FillProperties(string xml)
        {
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                var TitleAndResponsebility = nodes.Elements("varfield")
                    .Where(x => ((string) x.Attribute("id")).Equals("245")).Elements("subfield");

                Title = TitleAndResponsebility.Where(x => ((string) x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();
            }
        }

        public static Document GetDocumentFromFindDocXml(string xml)
        {
            var document = new Document();
            document.FillProperties(xml);
            return document;
        }
    }
}