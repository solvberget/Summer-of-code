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
        public bool IsFiction { get; set; }
        public string Language { get; set; }
        public string DocumentType { get; set; }
        public string LocationCode { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public IEnumerable<string> InvolvedPersons { get; set; }
        public string PlacePublished { get; set; }
        public string Publisher { get; set; }
        public int PublishedYear { get; set; }
        public string SeriesTitle { get; set; }
        public string SeriesNumber { get; set; }

        //public string Scope { get; set; }
        //public string Category { get; set; }
        //public string AlphabeticalStatement { get; set; }
        //public string DocumentNumber { get; set; }

        protected virtual void FillProperties(string xml)
        {
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants("oai_marc");

                //TargetGroup: set target group from character 22
                TargetGroup = GetFixfield(nodes, "008", 22, 22)[0];

                //IsFiction: set true/false from character 33
                IsFiction = GetFixfield(nodes, "008", 33, 33).Equals("1") ? true : false;
             
                //Language: Set language from characters 35-37
                Language = GetFixfield(nodes, "008", 35, 37);

                //DocumentType: Get varfield 019b
                DocumentType = GetVarfield(nodes, "019", "b");

                //LocationMode: Get varfield 090d
                LocationCode = GetVarfield(nodes, "090", "d");

                //Title, subtitle and involved persons: Get varfield 245abc
                Title = GetVarfield(nodes, "245", "a");
                SubTitle = GetVarfield(nodes, "245", "b");
                var involvedPersonsAsString = GetVarfield(nodes, "245", "c");
                if (involvedPersonsAsString != null)
                {
                    InvolvedPersons = involvedPersonsAsString.Split(';');
                }

                //Get publisher data form varfield 260abc
                var publisherData = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals("260")).Elements("subfield");

                //Get Publisher, place and year for publishing
                PlacePublished = GetVarfield(nodes, "260", "a");
                Publisher = GetVarfield(nodes, "260", "b");

                //Check and parse year published
                var publishedYearString = GetVarfield(nodes, "260", "c");
                if (publishedYearString != null)
                {
                    //Format may be "[2009]" or "2009.", trim if so
                    var regExp = new Regex(@"[a-zA-Z.\[\]]*(\d+)[a-zA-Z.\[\]]*");
                    var foundValue = regExp.Match(publishedYearString).Groups[1].ToString();
                    if (!string.IsNullOrEmpty(foundValue))
                        PublishedYear = int.Parse(foundValue);
                }

                //SeriesTitle: Get varfield 440av
                SeriesTitle = GetVarfield(nodes, "440", "a");
                SeriesNumber = GetVarfield(nodes, "440", "v");

            }
        }

        public static Document GetDocumentFromFindDocXml(string xml)
        {
            var document = new Document();
            document.FillProperties(xml);
            return document;
        }

        public string GetFixfield(IEnumerable<XElement> nodes, string id, int fromPos, int toPos)
        {
            var fixfield = nodes.Elements("fixfield").Where(x => ((string)x.Attribute("id")).Equals(id)).Select(x => x.Value).FirstOrDefault();
            
            if (fromPos == toPos)
            {
                return fixfield.ElementAt(fromPos).ToString();
            }
            else
            {
                return fixfield.Substring(fromPos, (toPos - fromPos) + 1);
            }
        }

        public string GetVarfield(IEnumerable<XElement> nodes, string id, string subfieldLabel)
        {
            var varfield = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals(id)).Elements("subfield");
            return varfield.Where(x => ((string)x.Attribute("label")).Equals(subfieldLabel)).Select(x => x.Value).FirstOrDefault();
        }

        public IEnumerable<string> GetVarfieldAsList(IEnumerable<XElement> nodes, string id, string subfieldLabel)
        {
            var varfield = nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals(id)).Elements("subfield");
            return varfield.Where(x => ((string) x.Attribute("label")).Equals(subfieldLabel)).Select(x => x.Value);
        }

    }

}
