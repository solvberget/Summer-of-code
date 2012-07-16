using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class CdPopular : Document
    {

        public Person Artist { get; set; }
        public string MusicGroup { get; set; }
        public string ExplanatoryAddition { get; set; }
        public string TypeAndNumberOfDiscs { get; set; }
        public string DiscContent { get; set; }
        public string Performers { get; set; }
        public IEnumerable<string> Genre { get; set; }
        public IEnumerable<Person> InvolvedPersons { get; set; }
        public IEnumerable<string> InvolvedMusicGroups { get; set; }
        public string SongTitlesOtherWritingForms { get; set; }

        protected override void FillPropertiesLight(string xml)
        {
            base.FillPropertiesLight(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");

                //Artist, check BSMARC field 100 for artist
                var nationality = GetVarfield(nodes, "100", "j");
                string nationalityLookupValue = null;
                if (nationality != null)
                    NationalityDictionary.TryGetValue(nationality, out nationalityLookupValue);
                Artist = new Person
                {
                    Name = GetVarfield(nodes, "100", "a"),
                    LivingYears = GetVarfield(nodes, "100", "d"),
                    Nationality = nationalityLookupValue ?? nationality,
                    Role = "Artist"
                };

                //If no artist, check BSMARC field 110 for MusicGroup
                if (Artist.Name == null)
                {
                    MusicGroup = GetVarfield(nodes, "110", "a");
                    ExplanatoryAddition = GetVarfield(nodes, "110", "q");
                }

            }
        }

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                TypeAndNumberOfDiscs = GetVarfield(nodes, "300", "a");
                DiscContent = GetVarfield(nodes, "505", "a");
                Performers = GetVarfield(nodes, "511", "a");
                Genre = GetVarfieldAsList(nodes, "652", "a");
                InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                InvolvedMusicGroups = GetVarfieldAsList(nodes, "710", "a");
                SongTitlesOtherWritingForms = GetVarfield(nodes, "740", "a");
            }

        }

        public new static CdPopular GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var document = new CdPopular();

            document.FillProperties(xml);

            return document;
        }

        public new static CdPopular GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var document = new CdPopular();

            document.FillPropertiesLight(xml);

            return document;
        }

    }
}