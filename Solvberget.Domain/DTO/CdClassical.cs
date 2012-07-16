using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class CdClassical : Document
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

        protected void FillPropertiesLight(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants("oai_marc");


            }
        }

        public new static CdClassical GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var document = new CdClassical();

            document.FillProperties(xml);

            return document;
        }

        public new static CdClassical GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var document = new CdClassical();

            document.FillPropertiesLight(xml);

            return document;
        }
    }
}