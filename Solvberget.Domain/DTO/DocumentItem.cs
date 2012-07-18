using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class DocumentItem
    {

        public string ItemKey { get; private set; }
        public string Branch { get; private set; }
        public string DepartmentCode { get; private set; }
        public string ItemStatus { get; private set; }
        public string PlacementCode { get; private set; }
        public bool OnHold { get; private set; }
        public bool InTransit { get; private set; }
        public string LoanDueDate { get; private set; }
        public string LoanDueHour { get; private set; }

        public static IEnumerable<DocumentItem> GetDocumentItemsFromXml(string itemDataXml)
        {
            
            var xmlDoc = XDocument.Parse(itemDataXml);

            var documentItems = (from item in xmlDoc.Root.Descendants("item")
                select new DocumentItem()
                {
                    ItemKey = GetValueFromXElement(item, "rec-key"),
                    Branch = GetValueFromXElement(item, "sub-library"),
                    DepartmentCode = GetValueFromXElement(item, "collection"),
                    ItemStatus = GetValueFromXElement(item, "item-status"),
                    PlacementCode = GetValueFromXElement(item, "call-no-1"),
                    OnHold = "Y".Equals(GetValueFromXElement(item, "on-hold")) ? true : false,
                    InTransit = "Y".Equals(GetValueFromXElement(item, "loan-in-transit")) ? true : false,
                    LoanDueDate = GetValueFromXElement(item, "loan-due-date"),
                    LoanDueHour = GetValueFromXElement(item, "loan-due-hour")
                });
                
            return documentItems;

        }

        private static string GetValueFromXElement(XElement item, string element)
        {
            var returnElement = item.Element(element);
            return returnElement == null ? null : returnElement.Value;
        }

        private static Dictionary<string, string> DepartmentCodeDictionary = new Dictionary<string, string>() {};

    }
}
