using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class DocumentItem
    {

        public string ItemKey { get; private set; }
        public string Branch { get; private set; }
        public string Department { get; private set; }
        public string ItemStatus { get; private set; }
        public string PlacementCode { get; private set; }
        public bool OnHold { get; private set; }
        public bool InTransit { get; private set; }
        public string LoanStatus { get; private set; }
        public DateTime? LoanDueDate { get; private set; }

        public static IEnumerable<DocumentItem> GetDocumentItemsFromXml(string itemDataXml)
        {

            var xmlDoc = XDocument.Parse(itemDataXml);

            if (xmlDoc.Root != null)
            {
               
                var documentItems = new List<DocumentItem>();

                var items = xmlDoc.Root.Descendants("item");
                
                foreach(var item in items)
                {
                    var docItem = new DocumentItem();
                    docItem.ItemKey = GetValueFromXElement(item, "rec-key");
                    docItem.ItemKey = GetValueFromXElement(item, "rec-key");
                    docItem.Branch = GetValueFromXElement(item, "sub-library");
                    string dep = string.Empty;
                    DepartmentCodeDictionary.TryGetValue(GetValueFromXElement(item, "collection"), out dep);
                    docItem.Department = dep;
                    docItem.ItemStatus = GetValueFromXElement(item, "item-status");
                    docItem.PlacementCode = GetValueFromXElement(item, "call-no-1");
                    docItem.OnHold = "Y".Equals(GetValueFromXElement(item, "on-hold")) ? true : false;
                    docItem.InTransit = "Y".Equals(GetValueFromXElement(item, "loan-in-transit")) ? true : false;
                    docItem.LoanStatus = GetValueFromXElement(item, "loan-status");
                    var loanDueDateAsString = GetValueFromXElement(item, "loan-due-date");
                    if (loanDueDateAsString != null)
                        docItem.LoanDueDate = ParseLoanDate(loanDueDateAsString);
                    documentItems.Add(docItem);
                }

                return documentItems;
            
            }

            return new List<DocumentItem>();

        }

        private static string GetValueFromXElement(XElement item, string element)
        {
            var returnElement = item.Element(element);
            return returnElement == null ? null : returnElement.Value;
        }

        private static DateTime ParseLoanDate(string date)
        {
            return DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        private static readonly Dictionary<string, string> DepartmentCodeDictionary = new Dictionary<string, string>()
                                                                                 {
                                                                                     { "KULT", "Kulturbiblioteket" },
                                                                                     { "FAKTA", "Faktabiblioteket"},
                                                                                     { "BARN", "Barne- og ungdomsbiblioteket" },
                                                                                     { "MUSIK", "Musikkbiblioteket "},
                                                                                     { "MAG", "Magasin" },
                                                                                     { "VOKS", "Voksenavdelingen" },
                                                                                     { "ATIDS", "Tidsskriftsavdelingen" }
                                                                                 };
    }
}
