using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class DocumentItemRepository
    {
        public static IEnumerable<DocumentItem> GetDocumentItemsFromXml(string itemDataXml, string itemCircDataXml, IRulesRepository rulesRepository)
        {
            var documentItems = new List<DocumentItem>();

            var xmlDoc = XDocument.Parse(itemDataXml);
            if (xmlDoc.Root != null)
            {

                var items = xmlDoc.Root.Descendants("item");

                foreach (var item in items)
                {
                    var docItem = new DocumentItem();
                    docItem.ItemKey = GetValueFromXElement(item, "rec-key");
                    docItem.Branch = GetValueFromXElement(item, "sub-library");
                    var dep = string.Empty;
                    DepartmentCodeDictionary.TryGetValue(GetValueFromXElement(item, "collection"), out dep);
                    docItem.Department = dep;
                    docItem.ItemStatus = GetValueFromXElement(item, "item-status");
                    docItem.PlacementCode = GetValueFromXElement(item, "call-no-1");
                    docItem.OnHold = "Y".Equals(GetValueFromXElement(item, "on-hold"));
                    docItem.InTransit = "Y".Equals(GetValueFromXElement(item, "loan-in-transit"));
                    docItem.LoanStatus = GetValueFromXElement(item, "loan-status");
                    docItem.Barcode = GetValueFromXElement(item, "barcode");
                    var loanDueDateAsString = GetValueFromXElement(item, "loan-due-date");
                    if (loanDueDateAsString != null)
                        docItem.LoanDueDate = ParseLoanDate(loanDueDateAsString);

                    docItem.ItemAdmKey = docItem.ItemKey.Substring(0, 9);
                    docItem.ItemKeySequence = docItem.ItemKey.Substring(9, 6);

                    documentItems.Add(docItem);
                }
            }

            xmlDoc = XDocument.Parse(itemCircDataXml);
            if (xmlDoc.Root != null)
            {
                var items = xmlDoc.Root.Descendants("item-data");

                foreach (var item in items)
                {
                    var barcode = GetValueFromXElement(item, "barcode");

                    var itemStatusText = GetValueFromXElement(item, "loan-status");
                    var dueDateText = GetValueFromXElement(item, "due-date");

                    var processText = itemStatusText ?? dueDateText;

                    var docItem =
                        documentItems.Where(x => x.Barcode.Equals(barcode)).Select(x => x).ToList().FirstOrDefault();
                    if (docItem != null)
                        docItem.ItemProcessStatusText = processText;

                    var noReqTmp = GetValueFromXElement(item, "no-requests");
                    if (string.IsNullOrEmpty(noReqTmp))
                    {
                        docItem.NoRequests = 0;
                    }
                    else
                    {
                        var noReqSplitted = noReqTmp.Split('(');
                        if (noReqSplitted[0] != null)
                        {
                            var tmp = noReqSplitted[0].Trim();
                            var noReqParsed = 0;
                            int.TryParse(tmp, out noReqParsed);
                            docItem.NoRequests = noReqParsed;
                        }
                    }

                }

            }
            foreach (var documentItem in documentItems)
            {
                documentItem.IsReservable = IsReservableItem(documentItem, rulesRepository);
            }
            return documentItems;

        }

        private static bool IsReservableItem(DocumentItem docItem, IRulesRepository rulesRepository)
        {

            var processText = docItem.ItemProcessStatusText;
            var itemStatusCode = docItem.ItemStatus;
            var rules = rulesRepository.GetItemRules();

            // No rules, no reservations
            if (rules == null)
                return false;

            // Nothing to get rules from, no reservation
            if (itemStatusCode == null && processText == null)
                return false;

            // Check rule for matching item status
            var itemStatusCodeRule = rules.Where(x => x.ItemStatus.Equals(itemStatusCode)).Select(x => x).ToList().FirstOrDefault();

            if (itemStatusCode != null && itemStatusCodeRule != null && !itemStatusCodeRule.CanReserve)
                return false;

            // Check rule for matching processText
            ItemRule ruleFromProcessText = null;
            if (processText != null)
                ruleFromProcessText = rules.Where(x => x.ProcessStatusText.Equals(processText)).Select(x => x).ToList().FirstOrDefault();

            if (processText != null && ruleFromProcessText != null && !ruleFromProcessText.CanReserve)
                return false;

            // Check if processTextCode is legal if any
            if (ruleFromProcessText != null)
            {
                var processTextCode = ruleFromProcessText.ProcessStatusCode;

                string[] legalCodes = { "##", "EN", "NA", "NB", "UL", "IU" };
                if (!legalCodes.Contains(processTextCode))
                    return false;
            }

            // Legal!
            return true;

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
