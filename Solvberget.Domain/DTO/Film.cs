using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Film : Document
    {
        public string Ean { get; set; }
        public IEnumerable<string> SpokenLanguage { get; set; }
        public IEnumerable<string> SubtitleLanguage { get; set; }
        public string OriginalTitle { get; set; }
        public string Numbering { get; set; }
        public string PartTitle { get; set; }
        public string Edition { get; set; }
        public string ProductionYear { get; set; }
        public string TypeAndNumberOfDiscs { get; set; }
        public string Contents { get; set; }
        public string Actors { get; set; }
        public string AgeLimit { get; set; }
        public string NorwegianTitle { get; set; }
        public string Subject { get; set; }
        public string GeoraphicSubject { get; set; }
        public string CompositionType { get; set; }
        public string Genre { get; set; }
        public string SubjectWordsAdults { get; set; }
        public string SubjectWordsChildren { get; set; }
        public IEnumerable<string> ResponsiblePersons { get; set; }

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants();

                //EAN: Get from varfield 025a
                Ean = GetVarfield(nodes, "025", "a");

                //SpokenLanguage: Get from varfield 041a
                
                var spokenString = GetVarfield(nodes, "041", "a");
                SpokenLanguage = spokenString.SplitByLength(3).ToList();


                //SubtitleLanguage: Get from varfield 041b
                var subtitleString = GetVarfield(nodes, "041", "b");
                SubtitleLanguage = subtitleString.SplitByLength(3);

                //OriginalTitle: Get from varfield 240a
                OriginalTitle = GetVarfield(nodes, "240", "a");

                //SequenceNumber: Get from varfield 245n
                Numbering = GetVarfield(nodes, "245", "n");

                //NameOfPart: Get from varfield 245p
                PartTitle = GetVarfield(nodes, "245", "p");

                //Edition: Get from varfield 250a
                Edition = GetVarfield(nodes, "250", "a");

                //ProductionYear: Get from varfield 260g
                ProductionYear = GetVarfield(nodes, "260", "g");

                //TypeAndNumberOfDiscs: Get froim varfield 300a
                TypeAndNumberOfDiscs = GetVarfield(nodes, "300", "a");

                //Contents: Get from varfield 505a
                Contents = GetVarfield(nodes, "505", "a");

                //Actor: Get from varfield 511a
                Actors = GetVarfield(nodes, "511", "a");

                //AgeLimit: Get from varfield 521a
                AgeLimit = GetVarfield(nodes, "521", "a");

                //NorwegianTitle: Get from varfield 572a
                NorwegianTitle = GetVarfield(nodes, "572", "a");

                //Subject: Get from varfield 650a
                Subject = GetVarfield(nodes, "650", "a");

                //GeographicSubject: Get from varfield 651a
                GeoraphicSubject = GetVarfield(nodes, "651", "a");

                //CompositionType: Get from varfield 652a
                CompositionType = GetVarfield(nodes, "652", "a");

                //Genre: Get from varfield 655a
                Genre = GetVarfield(nodes, "655", "a");

            }
        }

        public static Film GetFilmFromFindDocXml(string xml)
        {
            var film = new Film();

            film.FillProperties(xml);

            return film;
        }
    }
}
