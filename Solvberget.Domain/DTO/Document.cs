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
        public IEnumerable<string> Languages { get; set; }
        public IEnumerable<string> DocumentType { get; set; }
        public string LocationCode { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public IEnumerable<string> ResponsiblePersons { get; set; }
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
            
            FillPropertiesLight(xml);

            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants("oai_marc");

                TargetGroup = GetFixfield(nodes, "008", 22, 22)[0];
                IsFiction = GetFixfield(nodes, "008", 33, 33).Equals("1") ? true : false;

                //Only get languages (041a) if language in base equals "mul" 
                if (Language.Equals("mul"))
                {
                    Languages = GetVarfield(nodes, "041", "a").SplitByLength(3).ToList();
                }

                LocationCode = GetVarfield(nodes, "090", "d");
                SubTitle = GetVarfield(nodes, "245", "b");

                var involvedPersonsAsString = GetVarfield(nodes, "245", "c");
                if (involvedPersonsAsString != null)
                {
                    ResponsiblePersons = involvedPersonsAsString.Split(';');
                }

                PlacePublished = GetVarfield(nodes, "260", "a");
                Publisher = GetVarfield(nodes, "260", "b");

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

        public static Document GetDocumentFromFindDocXmlLight(string xml)
        {
            var document = new Document();
            document.FillPropertiesLight(xml);
            return document;
        }

        protected static string GetFixfield(IEnumerable<XElement> nodes, string id, int fromPos, int toPos)
        {
            var fixfield =
                nodes.Elements("fixfield").Where(x => ((string) x.Attribute("id")).Equals(id)).Select(x => x.Value).
                    FirstOrDefault();

            if (fromPos == toPos)
            {
                return fixfield.ElementAt(fromPos).ToString();
            }
            else
            {
                return fixfield.Substring(fromPos, (toPos - fromPos) + 1);
            }
        }

        protected static string GetVarfield(IEnumerable<XElement> nodes, string id, string subfieldLabel)
        {
            var varfield =
                nodes.Elements("varfield").Where(x => ((string) x.Attribute("id")).Equals(id)).Elements("subfield");
            return
                varfield.Where(x => ((string) x.Attribute("label")).Equals(subfieldLabel)).Select(x => x.Value).
                    FirstOrDefault();
        }

        protected static IEnumerable<string> GetVarfieldAsList(IEnumerable<XElement> nodes, string id,
                                                               string subfieldLabel)
        {
            var varfield =
                nodes.Elements("varfield").Where(x => ((string) x.Attribute("id")).Equals(id)).Elements("subfield");
            return varfield.Where(x => ((string) x.Attribute("label")).Equals(subfieldLabel)).Select(x => x.Value);
        }

        protected static string GetSubFieldValue(XElement varfield, string label)
        {
            return
                varfield.Elements("subfield").Where(x => ((string) x.Attribute("label")).Equals(label)).Select(
                    x => x.Value).FirstOrDefault();
        }

        protected static IEnumerable<Person> GeneratePersonsFromXml(IEnumerable<XElement> nodes, string id)
        {

            var persons = new List<Person>();

            var varfields = nodes.Elements("varfield").Where(x => ((string) x.Attribute("id")).Equals(id)).ToList();

            foreach (var varfield in varfields)
            {
                var person = new Person()
                                 {
                                     Name = GetSubFieldValue(varfield, "a"),
                                     LivingYears = GetSubFieldValue(varfield, "d"),
                                     Nationality = GetSubFieldValue(varfield, "j"),
                                     Role = GetSubFieldValue(varfield, "e"),
                                     ReferredWork = GetSubFieldValue(varfield, "t")
                                 };

                persons.Add(person);

            }

            return persons;

        }

        protected static IEnumerable<Organization> GenerateOrganizationsFromXml(IEnumerable<XElement> nodes, string id)
        {

            var organizations = new List<Organization>();

            var varfields = nodes.Elements("varfield").Where(x => ((string) x.Attribute("id")).Equals(id)).ToList();

            foreach (var varfield in varfields)
            {
                var org = new Organization()
                              {
                                  Name = GetSubFieldValue(varfield, "a"),
                                  UnderOrganization = GetSubFieldValue(varfield, "b"),
                                  Role = GetSubFieldValue(varfield, "e"),
                                  FurtherExplanation = GetSubFieldValue(varfield, "q"),
                                  ReferencedPublication = GetSubFieldValue(varfield, "t")
                              };

                organizations.Add(org);

            }

            return organizations;

        }

        protected virtual void FillPropertiesLight(string xml)
        {
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                Language = GetFixfield(nodes, "008", 35, 37);
                DocumentType = GetVarfield(nodes, "019", "b").Split(';');
                Title = GetVarfield(nodes, "245", "a");
                var publishedYearString = GetVarfield(nodes, "260", "c");
                if (publishedYearString != null)
                {
                    //Format may be "[2009]" or "2009.", trim if so
                    var regExp = new Regex(@"[a-zA-Z.\[\]]*(\d+)[a-zA-Z.\[\]]*");
                    var foundValue = regExp.Match(publishedYearString).Groups[1].ToString();
                    if (!string.IsNullOrEmpty(foundValue))
                        PublishedYear = int.Parse(foundValue);
                }

            }
        }

    }
}
