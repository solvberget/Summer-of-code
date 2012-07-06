using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class CdClassical : Cd
    {
        protected override void FillProperties(string xml)
        {
        }

        protected override void FillPropertiesLight(string xml)
        {
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