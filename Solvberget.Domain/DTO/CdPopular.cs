using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class CdPopular : Document
    {

        public string Artist { get; set; }
        public string MusicGroup { get; set; }
        public string ExplanatoryAddition { get; set; }
        public string Label { get; set; }

        public string TypeAndNumberOfDiscs { get; set; }


        public string GeneralInformation { get; set; }
        public string DiscContent { get; set; }
        public string Genre { get; set; }
        public string ResponsiblePersons { get; set; }

        public string SongTitles { get; set; }
        public string Group { get; set; }
        public string SongTitlesOtherWritingForms { get; set; }




        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants("oai_marc");

                Artist = GetVarfield(nodes,"100", "a");

                MusicGroup = GetVarfield(nodes, "110", "a");
                ExplanatoryAddition = GetVarfield(nodes, "110", "q");

                Label = GetVarfield(nodes, "260", "b");

                TypeAndNumberOfDiscs = GetVarfield(nodes, "300", "a");

                GeneralInformation = GetVarfield(nodes, "500", "a");
                DiscContent = GetVarfield(nodes, "505", "a");
                Genre = GetVarfield(nodes, "652", "a");
                ResponsiblePersons = GetVarfield(nodes, "700", "a");

                SongTitles = GetVarfield(nodes, "700", "t");
                Group = GetVarfield(nodes, "710", "a");
                SongTitlesOtherWritingForms = GetVarfield(nodes, "740", "a");

            }

        }

        protected void FillPropertiesLight(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants("oai_marc");


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