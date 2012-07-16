using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class SheetMusic : Document
    {
        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants("oai_marc");




            }
        }

        protected override void FillPropertiesLight(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants("oai_marc");


            }
        }

        public new static Book GetObjectFromFindDocXmlBsMarc(string xml)
        {
            return null;
        }

        public new static Book GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            return null;
        }
    }
}