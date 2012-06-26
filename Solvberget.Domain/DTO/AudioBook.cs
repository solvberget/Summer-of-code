using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class AudioBook : Document
    {
        public string Isbn { get; set; }
        public string ClassificationNumber { get; set; }
        public string Author { get; set; }
        public string AuthorLivingYears { get; set; }
        public string AuthorNationality { get; set; }
        public string Numbering { get; set; }
        public string PartTitle { get; set; }
        public string Edition { get; set; }
        public string TypeAndNumberOfDiscs { get; set; }
        public string Subject { get; set; }

        public IEnumerable<string> ReferencedPlaces { get; set; }
        public IEnumerable<string> Genre { get; set; }
        
        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants();

                Isbn = GetVarfield(nodes, "020", "a");
                ClassificationNumber = GetVarfield(nodes, "090", "c");
                Author = GetVarfield(nodes, "100", "a");
                AuthorLivingYears = GetVarfield(nodes, "100", "d");
                AuthorNationality = GetVarfield(nodes, "100", "j");
                Numbering = GetVarfield(nodes, "245", "n");
                PartTitle = GetVarfield(nodes, "245", "p");
                Edition = GetVarfield(nodes, "250", "a");
                TypeAndNumberOfDiscs = GetVarfield(nodes, "300", "a");
                Subject = GetVarfield(nodes, "650", "a");
                ReferencedPlaces = GetVarfieldAsList(nodes, "651", "a");
                Genre = GetVarfieldAsList(nodes, "655", "a");
            }
        }


        public static AudioBook GetAudioBookFromFindDocXml(string xml)
        {
            var audioBook = new AudioBook();

            audioBook.FillProperties(xml);

            return audioBook;
        }
    }
}
