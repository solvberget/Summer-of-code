using System.Collections.Generic;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Cd : Document
    {
        public new int StandardLoanTime { get { return 18; } }
        public Person ArtistOrComposer { get; set; }
        public string MusicGroup { get; set; }
        public string ExplanatoryAddition { get; set; }
        public string TypeAndNumberOfDiscs { get; set; }
        public IEnumerable<string> DiscContent { get; set; }
        public string Performers { get; set; }
        public IEnumerable<string> CompositionTypeOrGenre { get; set; }
        public string MusicalLineup { get; set; }
        public IEnumerable<Person> InvolvedPersons { get; set; }
        public IEnumerable<string> InvolvedMusicGroups { get; set; }
        public string SongTitlesOtherWritingForms { get; set; }

        public new static Cd GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var document = new Cd();

            document.FillProperties(xml);

            return document;
        }

        public new static Cd GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var document = new Cd();

            document.FillPropertiesLight(xml);

            return document;
        }

        public string ArtistOrGroupName { get 
            {
                if (ArtistOrComposer != null && ArtistOrComposer.Name != null) return ArtistOrComposer.Name;
                return MusicGroup ?? "Ukjent artist";
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
                var discContent = GetVarfield(nodes, "505", "a");

                if( discContent != null)
                    DiscContent = TrimContentList(discContent);

                Performers = GetVarfield(nodes, "511", "a");
                CompositionTypeOrGenre = GetVarfieldAsList(nodes, "652", "a");
                MusicalLineup = GetVarfield(nodes, "658", "a");
                InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                InvolvedMusicGroups = GetVarfieldAsList(nodes, "710", "a");
                SongTitlesOtherWritingForms = GetVarfield(nodes, "740", "a");
            }

        }

        protected override void FillPropertiesLight(string xml)
        {
            base.FillPropertiesLight(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");

                //ArtistOrComposer, check BSMARC field 100
                var nationality = GetVarfield(nodes, "100", "j");
                string nationalityLookupValue = null;
                if (nationality != null)
                    NationalityDictionary.TryGetValue(nationality, out nationalityLookupValue);
                ArtistOrComposer = new Person
                {
                    Name = GetVarfield(nodes, "100", "a"),
                    LivingYears = GetVarfield(nodes, "100", "d"),
                    Nationality = nationalityLookupValue ?? nationality,
                    Role = "ArtistOrComposer"
                };


                MainResponsible = ArtistOrComposer;


                //If no ArtistOrCompose, check BSMARC field 110 for MusicGroup
                if (ArtistOrComposer.Name == null)
                {
                    MusicGroup = GetVarfield(nodes, "110", "a");
                    ExplanatoryAddition = GetVarfield(nodes, "110", "q");
                    MainResponsible = MusicGroup;
                }
               
                if (ArtistOrComposer.Name != null)
                    ArtistOrComposer.InvertName(ArtistOrComposer.Name);
            }
        }

        protected override string GetCompressedString()
        {
            var returnValue = ArtistOrGroupName;

            if (returnValue.Equals("Ukjent artist"))
            {
                return base.GetCompressedString();
            }

            if (PublishedYear != 0)
            {
                returnValue += " (" + PublishedYear + ")";
            }

            return returnValue;
        }  
    }
}