using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Document
    {
        public char TargetGroup { get; set; }
        public string Language { get; set; }
        public string DocumentType { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public IEnumerable<string> InvolvedPersons { get; set; }
        public string PlacePublished { get; set; }
        public string Publisher { get; set; }
        public int PublishedYear { get; set; }

        //public string Scope { get; set; }
        //public string Category { get; set; }
        //public string AlphabeticalStatement { get; set; }
        //public string DocumentNumber { get; set; }

        protected virtual void FillProperties(string xml)
        {
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants();

                //Get complete information code string from fixfield 008
                var informationCodeString = nodes.Elements("fixfield").Where(x => ((string)x.Attribute("id")).Equals("008")).Select(x => x.Value).FirstOrDefault();

                //TargetGroup: set target group from character 22
                TargetGroup = informationCodeString.ElementAt(22);

                //Language: Set language from characters 35-37
                Language = informationCodeString.Substring(35, 3);

                //DocumentType: Get varfield 019b
                var docType = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("019")).Elements("subfield");
                DocumentType = docType.Where(x => ((string)x.Attribute("label")).Equals("b")).Select(x => x.Value).FirstOrDefault();

                //Title, subtitle and involved persons: Get varfield 245abc
                var TitleAndResponsebility = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("245")).Elements("subfield");

                //Set title, subtitle and involved persons
                if (TitleAndResponsebility != null)
                {
                    Title = TitleAndResponsebility.Where(x => ((string)x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();
                    SubTitle = TitleAndResponsebility.Where(x => ((string)x.Attribute("label")).Equals("b")).Select(x => x.Value).FirstOrDefault();
                    InvolvedPersons = TitleAndResponsebility.Where(x => ((string)x.Attribute("label")).Equals("c")).Select(x => x.Value).FirstOrDefault().Split(';');
                }

                //Get publisher data form varfield 260abc
                var publisherData = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("260")).Elements("subfield");

                //Set Publisher, place and year for publishing
                if (publisherData != null)
                {
                    PlacePublished = publisherData.Where(x => ((string)x.Attribute("label")).Equals("a")).Select(x => x.Value).FirstOrDefault();
                    Publisher = publisherData.Where(x => ((string)x.Attribute("label")).Equals("b")).Select(x => x.Value).FirstOrDefault();

                    //Check and parse year published
                    var publishedYearString = publisherData.Where(x => ((string)x.Attribute("label")).Equals("c")).Select(x => x.Value).FirstOrDefault();
                    if (publishedYearString != null)
                    {
                        //Format may be "[2009]" or "2009.", trim if so
                        var regExp = new Regex(@"[a-zA-Z.\[\]]*(\d+)[a-zA-Z.\[\]]*");
                        var foundValue = regExp.Match(publishedYearString).Groups[1].ToString();
                        if (!string.IsNullOrEmpty(foundValue))
                        {
                            PublishedYear = int.Parse(foundValue);
                        }

                    }

                }
                
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