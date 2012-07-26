using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class UserInfo
    {
        public string Id { get; set; }
        public bool IsAuthorized { get; set; }
        public string BorrowerId { get; set; }
        public string Name { get; set; }
        public string PrefixAddress { get; set; }
        public string StreetAddress { get; set; }
        public string CityAddress { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string HomePhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string CashLimit { get; set; }
        public string HomeLibrary { get; set; }
        public string Balance { get; set; }
        public IEnumerable<Fine> Fines { get; set; }
        public IEnumerable<Fine> ActiveFines { get; set; }
        public IEnumerable<Loan> Loans { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }


        public void FillProperties(string xml)
        {

            XDocument xdoc;
            try
            {
                xdoc = XDocument.Parse(xml);
            }
            catch
            {
                return;
            }

            if (xdoc.Root == null) return;

            var xElement = xdoc.Element("bor-info");
            if (xElement == null) return;
            var xElementRecord = xElement.Element("z303");
            if (xElementRecord == null) return;

            var xElementField = xElementRecord;

            Id = GetXmlValue(xElementRecord, "z303-id");
            Name = GetFormattedName(GetXmlValue(xElementRecord, "z303-name"));

            DateOfBirth = GetFormattedDate(GetXmlValue(xElementRecord, "z303-birth-date"));

            HomeLibrary = GetXmlValue(xElementRecord, "z303-home-library");

            xElementRecord = xElement.Element("z304");
            if (xElementRecord == null) return;

            PrefixAddress = GetXmlValue(xElementRecord, "z304-address-0");
            StreetAddress = GetXmlValue(xElementRecord, "z304-address-1");
            CityAddress = GetXmlValue(xElementRecord, "z304-address-3");
            Zip = GetXmlValue(xElementRecord, "z304-zip");
            Email = GetXmlValue(xElementRecord, "z304-email-address");
            HomePhoneNumber = GetXmlValue(xElementRecord, "z304-telephone");
            CellPhoneNumber = GetXmlValue(xElementRecord, "z304-telephone-3");

            xElementRecord = xElement.Element("z305");
            if (xElementRecord == null) return;

            CashLimit = GetXmlValue(xElementRecord, "z305-cash-limit");

            xElementRecord = xElement.Element("balance");
            if (xElementRecord == null) return;

            Balance = xElementRecord.Value;

            
            // Put all the reservations of the borrower into a list of Reservation objects
            xElementRecord = xElement.Element("item-h");
            if (xElementRecord != null)
            {

                var pickupLocation = "";
                var holdRequestFrom = "";
                var holdRequestTo = "";
                var reservations = new List<Reservation>();

                var reservationVarfields = xElement.Elements("item-h").ToList();

                foreach (var varfield in reservationVarfields)
                {

                    //Get information from table z37
                    xElementField = varfield.Element("z37");
                    if (xElementField != null)
                    {
                        pickupLocation = GetXmlValue(xElementField, "z37-pickup-location");
                        if (pickupLocation == "Hovedbibl.")
                            pickupLocation = "Hovedbiblioteket";

                        holdRequestFrom = GetFormattedDate(GetXmlValue(xElementField, "z37-request-date"));

                        holdRequestTo = GetFormattedDate(GetXmlValue(xElementField, "z37-end-request-date"));

                    }
                    
                    //Get information from table z13
                    xElementField = varfield.Element("z13");
                    if (xElementField != null)
                    {
                        var docNumber = GetXmlValue(xElementField, "z13-doc-number");

                        var docTitle = GetXmlValue(xElementField, "z13-title");

                        var reservation = new Reservation()
                        {
                            DocumentNumber = docNumber,
                            DocumentTitle = docTitle,
                            PickupLocation = pickupLocation,
                            HoldRequestFrom = holdRequestFrom,
                            HoldRequestTo = holdRequestTo,
                        };

                        reservations.Add(reservation);
                    }
                }




                Reservations = reservations;
            }


            //Put all the loans for the borrower into a list of Loan objects

            xElementRecord = xElement.Element("item-l");
            if (xElementRecord != null)
            {
                var itemSequence = "";
                var subLibrary = "";
                var orgDueDate = "";
                var loanDate = "";
                var loanHour = "";
                var dueDate = "";
                var itemStatus = "";
                var barcode = "";
                var docNumber = "";


                var loans = new List<Loan>();

                var loanVarfields = xElement.Elements("item-l").ToList();

                foreach (var varfield in loanVarfields)
                {
                    //Get information from table z36
                    xElementField = varfield.Element("z36");
                    if (xElementField != null)
                    {

                        

                        subLibrary = GetXmlValue(xElementField, "z36-sub-library");
                        if (subLibrary == "Hovedbibl.")
                            subLibrary = "Hovedbiblioteket";

                        orgDueDate = GetFormattedDate(GetXmlValue(xElementField, "z36-original-due-date"));

                        loanDate = GetFormattedDate(GetXmlValue(xElementField, "z36-loan-date"));

                        loanHour = GetXmlValue(xElementField, "z36-loan-hour");

                        dueDate = GetFormattedDate(GetXmlValue(xElementField, "z36-due-date"));
                    }
                    
                    //Get information from table z30
                    xElementField = varfield.Element("z30");
                    if (xElementField != null)
                    {
                        docNumber = GetXmlValue(xElementField, "z30-doc-number");

                        itemSequence = GetXmlValue(xElementField, "z30-item-sequence");

                        itemStatus = GetXmlValue(xElementField, "z30-item-status");

                        barcode = GetXmlValue(xElementField, "z30-barcode");

                    }

                    //Get information from table z13
                    xElementField = varfield.Element("z13");
                    if (xElementField != null)
                    {
                        var docTitle = GetXmlValue(xElementField, "z13-title");

                        var loan = new Loan()
                                       {
                                           DocumentNumber = docNumber,
                                           ItemSequence = itemSequence,
                                           Barcode = barcode,
                                           DocumentTitle = docTitle,
                                           SubLibrary = subLibrary,
                                           OriginalDueDate = orgDueDate,
                                           ItemStatus = itemStatus,
                                           LoanDate = loanDate,
                                           LoanHour = loanHour,
                                           Material = null,
                                           DueDate = dueDate
                                       };

                        loans.Add(loan);
                    }
                }
                Loans = loans;
            }


            //Put all fines connected to the borrower in a list
            xElementRecord = xElement.Element("fine");
            if (xElementRecord != null)
            {
                var fines = new List<Fine>();
                var activeFines = new List<Fine>();

                var varfields = xElement.Elements("fine").ToList();
                foreach (var varfield in varfields)
                {

                    var temp = varfield.Elements().ToList();
                    
                    //Get information from table z31
                    xElementField = xElementRecord.Element("z31");
                    if (xElementField == null) return;

                    //Get a number from the data in Sum field
                    var sumAsString = GetXmlValue(temp.ElementAt(0), "z31-sum");
                    double sum = 0;
                    if (sumAsString != null)
                    {
                        //Format may be "[2009]" or "(30.00)", trim if so
                        var regExp = new Regex(@"[a-zA-Z\[\]]*(\d+)[a-zA-Z\[\]]*");
                        var foundValue = regExp.Match(sumAsString).Groups[1].ToString();
                        if (!string.IsNullOrEmpty(foundValue))
                            sum = double.Parse(foundValue);
                    }

                    var date = GetFormattedDate(GetXmlValue(temp.ElementAt(0), "z31-date"));

                    var status = GetXmlValue(temp.ElementAt(0), "z31-status");

                    if (status == "Not paid by/credited to patron")
                        status = "Ikke betalt ";
                    
                    var description = GetXmlValue(temp.ElementAt(0), "z31-type");
                    string descriptionLookupValue = null;
                    if (description != null)
                        TypeOfFineDictionary.TryGetValue(description, out descriptionLookupValue);

                    var creditDebit = Convert.ToChar(GetXmlValue(temp.ElementAt(0), "z31-credit-debit"));

                    //Get information from table z13, givent that there is more than one node in temp
                    var docId = "";
                    var docTitle = "";
                    if (temp.Count > 1)
                    {
                        docId = GetXmlValue(temp.ElementAt(1), "z13-doc-number") ?? docId;

                        docTitle = GetXmlValue(temp.ElementAt(1), "z13-title") ?? docTitle;
                    }

                    var fine = new Fine()
                                   {
                                       Date = date,
                                       Status = status,
                                       CreditDebit = creditDebit,
                                       Sum = sum,
                                       Description = descriptionLookupValue ?? description,
                                       DocumentNumber = docId,
                                       DocumentTitle = docTitle
                                   };
                    fines.Add(fine);
                    
                    if (fine.Status != "Cancelled" && fine.Status != "Paid")
                        activeFines.Add(fine);
                        
                }
                Fines = fines;
                ActiveFines = activeFines;
            }
        }

        private static string GetXmlValue(XElement node, string tag)
        {
            var xElement = node.DescendantsAndSelf(tag).FirstOrDefault();
            return xElement == null ? string.Empty : xElement.Value;
        }

        private static string GetFormattedName(string name)
        {

            if (!name.Contains(','))
                return name;

            var subNames = name.Split(',');

            string formattedName;
            if (subNames.Length > 1)
                formattedName = subNames[1] + " " + subNames[0];
            else
                formattedName = subNames[0];

            formattedName = formattedName.Trim();

            return formattedName;
        }

        private static string GetFormattedDate(string dateOfBirth)
        {
            if (dateOfBirth.Length > 7)
            {
                var year = dateOfBirth.Substring(0,4);
                var month = dateOfBirth.Substring(4, 2);
                var day = dateOfBirth.Substring(6, 2);
                return day + "." + month + "." + year;
            }

            return string.Empty;
        }

        protected static readonly Dictionary<string, string> TypeOfFineDictionary = new Dictionary<string, string>
                                {
                                    {"0", "Betalt"},
                                    {"3", "For sent levert"},
                                    {"6", "Legitimasjonslån"},
                                    {"8", "Nytt lånekort"},
                                    {"9", "Kopier"},
                                    {"17", "Regningsgebyr voksen"},
                                    {"18", "Ufullstendig innlevering barn"},
                                    {"19", "Ufullstendig innelvering voksne"},
                                    {"20", "Regningsgebyr barn"},
                                    {"21", "Utskrift sort/hvit"},
                                    {"22", "Utskrift farge"},
                                    {"23", "Delbetaling"},
                                    {"24", "Erstatning"},
                                    {"25", "Diverse"},
                                    {"40", "Materiell mistet, erstatningskrav er opprettet"},
                                    {"41", "Materiell mistet, erstatningskrav er opprettet"},
                                    {"42", "Materiell mistet, erstatningskrav er opprettet"},
                                    {"80", "1. purring"},
                                    {"81", "2. purring"},
                                    {"82", "3. purring"},
                                    {"83", "4. purring"},
                                    {"1000", "Betalt"}

                                };
    }
}
