using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Film : Document
    {
        public string Ean { get; set; }
        public string OriginalTitle { get; set; }
        public string Numbering { get; set; }
        public string PartTitle { get; set; }
        public string Edition { get; set; }
        public string ProductionYear { get; set; }
        public string TypeAndNumberOfDiscs { get; set; }
        public string Contents { get; set; }
        public IEnumerable<Person> Actors { get; set; }
        public string AgeLimit { get; set; }
        public string NorwegianTitle { get; set; }
        public string Subject { get; set; }
        public string CompositionType { get; set; }
        public string TypeOfMedia { get; set; }
        public IEnumerable<string> SubtitleLanguage { get; set; }
        public IEnumerable<string> ReferencedPlaces { get; set; }
        public IEnumerable<string> Genre { get; set; }
        public IEnumerable<Person> ReferredPersons { get; set; }
        public IEnumerable<Organization> ReferredOrganizations { get; set; }
        public IEnumerable<Person> InvolvedPersons { get; set; }
        public IEnumerable<Organization> InvolvedOrganizations { get; set; }

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants();

                Ean = GetVarfield(nodes, "025", "a");
                //Each subtitlelanguage is given as a 3-char long code, all put together to one string
                var subtitles = GetVarfield(nodes, "041", "b").SplitByLength(3).ToList();

                for (var i = 0; i < subtitles.Count(); i++)
                {
                    string subtitleLookupValue = null;
                    LanguageDictionary.TryGetValue(subtitles[i], out subtitleLookupValue);
                    subtitles[i] = subtitleLookupValue ?? subtitles[i];
                }

                SubtitleLanguage = subtitles;
                OriginalTitle = GetVarfield(nodes, "240", "a");
                Numbering = GetVarfield(nodes, "245", "n");
                PartTitle = GetVarfield(nodes, "245", "p");
                Edition = GetVarfield(nodes, "250", "a");
                ProductionYear = GetVarfield(nodes, "260", "g");
                TypeAndNumberOfDiscs = GetVarfield(nodes, "300", "a");
                Contents = GetVarfield(nodes, "505", "a");

                var actorsString = GetVarfield(nodes, "511", "a");
                var persons = new List<Person>();

                if (actorsString != null)
                {
                    var actorsList = actorsString.Split(':');
                    if (actorsList.Length > 1)
                    {
                        actorsList = actorsList[1].Split(',');
                    }


                    foreach (string personName in actorsList)
                    {
                        var p = new Person();
                        p.Name = personName.Trim();
                        p.InvertName(p.Name);
                        persons.Add(p);
                    }
                }

                Actors = persons;

                AgeLimit = GetVarfield(nodes, "521", "a");
                NorwegianTitle = GetVarfield(nodes, "572", "a");
                ReferredPersons = GeneratePersonsFromXml(nodes, "600");
                ReferredOrganizations = GenerateOrganizationsFromXml(nodes, "610");
                Subject = GetVarfield(nodes, "650", "a");
                ReferencedPlaces = GetVarfieldAsList(nodes, "651", "a");
                CompositionType = GetVarfield(nodes, "652", "a");
                Genre = GetVarfieldAsList(nodes, "655", "a");
                InvolvedOrganizations = GenerateOrganizationsFromXml(nodes, "710");
            }
        }

        protected override void FillPropertiesLight(string xml)
        {
            base.FillPropertiesLight(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants();
                OriginalTitle = GetVarfield(nodes, "240", "a");
                ProductionYear = GetVarfield(nodes, "260", "g");
                AgeLimit = GetVarfield(nodes, "521", "a");
                Genre = GetVarfieldAsList(nodes, "655", "a");
                InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                MainResponsible = ResponsiblePersons;

                var typeOfMedia = GetVarfield(nodes, "019", "b");
                if (typeOfMedia != null)
                {
                    if (typeOfMedia.Contains("ef"))
                    {
                        TypeOfMedia = "Blu-ray";
                    }
                    else if (typeOfMedia.Contains("ee"))
                    {
                        TypeOfMedia = "DVD";
                    }
                    else if (typeOfMedia.Contains("eg"))
                    {
                        TypeOfMedia = "3D";
                    }
                }
            }
        }

        public new static Film GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var film = new Film();

            film.FillProperties(xml);

            return film;
        }

        public new static Film GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var film = new Film();

            film.FillPropertiesLight(xml);

            return film;
        }
    }
}
