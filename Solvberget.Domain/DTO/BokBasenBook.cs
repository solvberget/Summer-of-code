using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public sealed class BokBasenBook
    {
        // Same property names as XML-doc
        // ReSharper disable InconsistentNaming
        public string Id { get; set; }
        public string Fsreview { get; set; }
        public string Publisher_text { get; set; }
        public string Contents { get; set; }
        public string Thumb_Cover_Picture { get; set; }
        public string Small_Cover_Picture { get; set; }
        public string Large_Cover_Picture { get; set; }
        public string Sound { get; set; }
        public string Extract { get; set; }
        public string Marc { get; set; }
        public string Reviews { get; set; }
        // ReSharper restore InconsistentNaming

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

            var xElement = xdoc.Element("BOOK_JACKETS");
            if (xElement == null) return;
            var xElementRecord = xElement.Element("RECORD");
            if (xElementRecord == null) return;

            var element = xElementRecord.FirstAttribute;
            if (element != null) Id = element.Value;

            Fsreview = GetXmlValue(xElementRecord, GetPropertyName(() => Fsreview));
            Publisher_text = GetXmlValue(xElementRecord, GetPropertyName(() => Publisher_text));
            Contents = GetXmlValue(xElementRecord, GetPropertyName(() => Contents));
            Thumb_Cover_Picture = GetXmlValue(xElementRecord, GetPropertyName(() => Thumb_Cover_Picture));
            Small_Cover_Picture = GetXmlValue(xElementRecord, GetPropertyName(() => Small_Cover_Picture));
            Large_Cover_Picture = GetXmlValue(xElementRecord, GetPropertyName(() => Large_Cover_Picture));
            Sound = GetXmlValue(xElementRecord, GetPropertyName(() => Sound));
            Extract = GetXmlValue(xElementRecord, GetPropertyName(() => Extract));
            Marc = GetXmlValue(xElementRecord, GetPropertyName(() => Marc));
            Reviews = GetXmlValue(xElementRecord, GetPropertyName(() => Reviews));
                

        }

        private static string GetXmlValue(XElement node, string tag)
        {
            var xElement = node.DescendantsAndSelf(tag.ToUpper()).FirstOrDefault();
            return xElement == null ? string.Empty : xElement.Value;
        }

        private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            return memberExpression != null ? memberExpression.Member.Name : string.Empty;
        }
    }
}
