using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Journal : Document
    {
        public string DateRegistered { get; set; }

        public string JournalsPerYear { get; set; }

        public string GeneralInformation { get; set; }
        public string InventoryInfomation { get; set; }
        public string Topic { get; set; }
        public string ReferencedPlaces { get; set; }
        public string ReferredPersons { get; set; }

        public string InvolvedOrganizations { get; set; }
        public string OtherTitles { get; set; }

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                DateRegistered = GetVarfield(nodes, "008", "00, 05");
                JournalsPerYear = GetVarfield(nodes, "310", "a");
                GeneralInformation = GetVarfield(nodes, "500", "a");
                InventoryInfomation = GetVarfield(nodes, "590", "a");
                Topic = GetVarfield(nodes, "650", "a");
                ReferencedPlaces = GetVarfield(nodes, "651", "a");
                ReferredPersons = GetVarfield(nodes, "700", "a");
                InvolvedOrganizations = GetVarfield(nodes, "710", "a");
                OtherTitles = GetVarfield(nodes, "740", "a");   
            }

        }

        protected override void FillPropertiesLight(string xml)
        {

            base.FillPropertiesLight(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                DateRegistered = GetVarfield(nodes, "008", "00, 05");
                JournalsPerYear = GetVarfield(nodes, "310", "a");
                GeneralInformation = GetVarfield(nodes, "500", "a");
                InventoryInfomation = GetVarfield(nodes, "590", "a");
                Topic = GetVarfield(nodes, "650", "a");
                ReferencedPlaces = GetVarfield(nodes, "651", "a");
                ReferredPersons = GetVarfield(nodes, "700", "a");
                InvolvedOrganizations = GetVarfield(nodes, "710", "a");
                OtherTitles = GetVarfield(nodes, "740", "a");  
            }
        }

        public new static Journal GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var domainObject = new Journal();

            domainObject.FillProperties(xml);

            return domainObject;
        }

        public new static Journal GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var domainObject = new Journal();

            domainObject.FillPropertiesLight(xml);

            return domainObject;
        }
    }
}