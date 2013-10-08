using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public sealed class BokElskereBook
    {
        // Same property names as XML-doc
        public string Id { get; set; }
        public string gjennomsnittelig_terningkast { get; set; }
        public string antall_sitater { get; set; }
        public string favorittprosent { get; set; }
        public string antall_favoritt { get; set; }
        public string link { get; set; }
        public string antall_diskusjoner { get; set; }
        public string antall_eiere { get; set; }
        public string antall_lesere { get; set; }
        public string antall_lister { get; set; }
        public string antall_terningkast { get; set; }


        public void FillProperties(string xml)
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(xml);
            }
            catch
            {
                return;
            }


            if (xdoc.Root == null) return;

            var xElement = xdoc.Element("response");
            if (xElement == null) return;
            var element = xElement.FirstAttribute;
            gjennomsnittelig_terningkast = GetXmlValue(xElement, GetPropertyName(() => gjennomsnittelig_terningkast));
            decimal terningkast;
            if (Decimal.TryParse(gjennomsnittelig_terningkast, NumberStyles.Any, CultureInfo.InvariantCulture, out terningkast))
            {
                var gjennomsnittelig_terningkast_decimal = Math.Round(terningkast, 1);
                gjennomsnittelig_terningkast = gjennomsnittelig_terningkast_decimal.ToString(CultureInfo.InvariantCulture);
            }
            
            //antall_sitater = GetXmlValue(xElement, GetPropertyName(() => antall_sitater));
            //favorittprosent = GetXmlValue(xElement, GetPropertyName(() => favorittprosent));
            //antall_favoritt = GetXmlValue(xElement, GetPropertyName(() => antall_favoritt));
            //link = GetXmlValue(xElement, GetPropertyName(() => link));
            //antall_diskusjoner = GetXmlValue(xElement, GetPropertyName(() => antall_diskusjoner));
            //antall_eiere = GetXmlValue(xElement, GetPropertyName(() => antall_eiere));
            //antall_lesere = GetXmlValue(xElement, GetPropertyName(() => antall_lesere));
            //antall_lister = GetXmlValue(xElement, GetPropertyName(() => antall_lister));
            //antall_terningkast = GetXmlValue(xElement, GetPropertyName(() => antall_terningkast));

        }

        private static string GetXmlValue(XElement node, string tag)
        {
            var xElement = node.DescendantsAndSelf(tag.ToLower()).FirstOrDefault();
            return xElement == null ? string.Empty : xElement.Value;
        }

        private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            return memberExpression != null ? memberExpression.Member.Name : string.Empty;
        }
    }
}
