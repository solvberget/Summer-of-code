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
        public IEnumerable<Notification> Notifications { get; set; }


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
            var xElementField = xElementRecord;
            if (xElementRecord != null)
            {

                Id = GetXmlValue(xElementRecord, "z303-id");
                Name = GetFormattedName(GetXmlValue(xElementRecord, "z303-name"));
                DateOfBirth = GetFormattedDate(GetXmlValue(xElementRecord, "z303-birth-date"));
                HomeLibrary = GetXmlValue(xElementRecord, "z303-home-library");
            }
            xElementRecord = xElement.Element("z304");

            if (xElementRecord != null)
            {
                PrefixAddress = GetXmlValue(xElementRecord, "z304-address-0");
                StreetAddress = GetXmlValue(xElementRecord, "z304-address-1");
                CityAddress = GetXmlValue(xElementRecord, "z304-address-3");
                Zip = GetXmlValue(xElementRecord, "z304-zip");
                Email = GetXmlValue(xElementRecord, "z304-email-address");
                HomePhoneNumber = GetXmlValue(xElementRecord, "z304-telephone");
                CellPhoneNumber = GetXmlValue(xElementRecord, "z304-telephone-3");
            }
            xElementRecord = xElement.Element("z305");
            if (xElementRecord != null)
            {
                CashLimit = GetXmlValue(xElementRecord, "z305-cash-limit");
            }
            xElementRecord = xElement.Element("balance");
            if (xElementRecord != null)
            {

                Balance = xElementRecord.Value;
            }

            // Put all the reservations of the borrower into a list of Reservation objects
            xElementRecord = xElement.Element("item-h");
            if (xElementRecord != null)
            {

                var pickupLocation = "";
                var holdRequestFrom = "";
                var holdRequestTo = "";
                var cancellationSequence = "";
                var itemSeq = "";
                var itemDocNumber = "";
                var holdRequestEnd = "";
                var reservationStatus = "";

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

                        reservationStatus = GetXmlValue(xElementField, "z37-status");
                        holdRequestFrom = GetFormattedDate(GetXmlValue(xElementField, "z37-request-date"));
                        holdRequestTo = GetFormattedDate(GetXmlValue(xElementField, "z37-end-request-date"));
                        cancellationSequence = GetXmlValue(xElementField, "z37-sequence");
                        itemSeq = GetXmlValue(xElementField, "z37-item-sequence");
                        itemDocNumber = GetXmlValue(xElementField, "z37-doc-number");
                        holdRequestEnd = GetFormattedDate(GetXmlValue(xElementField, "z37-end-hold-date"));
                    }

                    //Get information from table z13
                    xElementField = varfield.Element("z13");
                    if (xElementField == null) continue;
                    var docNumber = GetXmlValue(xElementField, "z13-doc-number");

                    docNumber = GetFormattedDocNumber(docNumber);
                    var docTitle = GetXmlValue(xElementField, "z13-title");


                    var reservation = new Reservation()
                    {
                        Status = reservationStatus,
                        DocumentNumber = docNumber,
                        DocumentTitle = docTitle,
                        PickupLocation = pickupLocation,
                        HoldRequestFrom = holdRequestFrom,
                        HoldRequestTo = holdRequestTo,
                        CancellationSequence = cancellationSequence,
                        ItemSeq = itemSeq,
                        ItemDocumentNumber = itemDocNumber,
                        HoldRequestEnd = holdRequestEnd,
                    };

                    reservations.Add(reservation);
                }



                reservations = reservations.OrderBy(x => x.HoldRequestTo).ToList();
                Reservations = reservations;
            }


            //Put all the loans for the borrower into a list of Loan objects

            xElementRecord = xElement.Element("item-l");
            if (xElementRecord != null)
            {
                var docNumber = "";
                var itemSequence = "";
                var subLibrary = "";
                var orgDueDate = "";
                var loanDate = "";
                var loanHour = "";
                var dueDate = "";
                var itemStatus = "";
                var barcode = "";
                var adminDocNumber = "";
                var docTitle = "";


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
                        adminDocNumber = GetXmlValue(xElementField, "z30-doc-number");

                        itemSequence = GetXmlValue(xElementField, "z30-item-sequence");

                        itemStatus = GetXmlValue(xElementField, "z30-item-status");

                        barcode = GetXmlValue(xElementField, "z30-barcode");

                    }

                    //Get information from table z13
                    xElementField = varfield.Element("z13");
                    if (xElementField != null)
                    {
                        docNumber = GetXmlValue(xElementField, "z13-doc-number");
                        docNumber = GetFormattedDocNumber(docNumber);
        
                        docTitle = GetXmlValue(xElementField, "z13-title");
                    }
                    var loan = new Loan()
                                   {
                                       DocumentNumber = docNumber,
                                       AdminisrtativeDocumentNumber = adminDocNumber,
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
                loans = loans.OrderBy(x => x.DueDate).ToList();
                Loans = loans;
            }

            //Put all fines connected to the borrower in a list
            xElementRecord = xElement.Element("fine");
            double sum = 0;
            var date = "";
            var status = "";
            var creditDebit = new char();
            var description = "";
            string descriptionLookupValue = null;

            if (xElementRecord != null)
            {
                var fines = new List<Fine>();
                var activeFines = new List<Fine>();

                var varfields = xElement.Elements("fine").ToList();
                foreach (var varfield in varfields)
                {
                    //Get information from table z31
                    xElementField = varfield.Element("z31");
                    if (xElementField != null)
                    {

                        //Get a number from the data in Sum field
                        var sumAsString = GetXmlValue(xElementField, "z31-sum");
                        if (sumAsString != null)
                        {
                            //Format may be "[2009]" or "(30.00)", trim if so
                            var regExp = new Regex(@"[a-zA-Z\[\]]*(\d+)[a-zA-Z\[\]]*");
                            var foundValue = regExp.Match(sumAsString).Groups[1].ToString();
                            if (!string.IsNullOrEmpty(foundValue))
                                sum = double.Parse(foundValue);
                        }

                        date = GetFormattedDate(GetXmlValue(xElementField, "z31-date"));

                        status = GetXmlValue(xElementField, "z31-status");

                        if (status == "Not paid by/credited to patron")
                            status = "Ikke betalt ";

                        description = GetXmlValue(xElementField, "z31-type");
                        if (description != null)
                            TypeOfFineDictionary.TryGetValue(description, out descriptionLookupValue);

                        creditDebit = Convert.ToChar(GetXmlValue(xElementField, "z31-credit-debit"));
                    }

                    //Get information from table z13, givent that there is more than one node in temp
                    xElementField = varfield.Element("z13");
                    var docId = "";
                    var docTitle = "";

                    if (xElementField != null)
                    {
                        docId = GetXmlValue(xElementField, "z13-doc-number") ?? docId;
                        docId = GetFormattedDocNumber(docId);

                        docTitle = GetXmlValue(xElementField, "z13-title");
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
            Notifications = GetNotification(this);


        }

        private static string GetFormattedDocNumber(string docNumber)
        {
            if (!string.IsNullOrEmpty(docNumber))
            {
                var docNumberStringBuilder = new StringBuilder(docNumber);
                for (var i = docNumber.Length; i < 9; i++)
                    docNumberStringBuilder.Insert(0, "0");

                docNumber = docNumberStringBuilder.ToString();
            }
            return docNumber;
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

        private static string GetFormattedDate(string date)
        {
            if (date.Length > 7)
            {
                var year = date.Substring(0, 4);
                var month = date.Substring(4, 2);
                var day = date.Substring(6, 2);
                return day + "." + month + "." + year;
            }

            return string.Empty;
        }

        private IEnumerable<Notification> GetNotification(UserInfo user)
        {
            var notifications = new List<Notification>();

            if (user.Loans != null)
            {
                foreach (var loan in user.Loans)
                {
                    var day = Convert.ToInt32(loan.DueDate.Substring(0, 2));
                    var month = Convert.ToInt32(loan.DueDate.Substring(3, 2));
                    var year = Convert.ToInt32(loan.DueDate.Substring(6, 4));

                    var dueDate = new DateTime(year, month, day);
                    var today = DateTime.Now;

                    TimeSpan span = dueDate.Subtract(today);
                    var timeLeft = span.Days;

                    if (timeLeft > 0 && timeLeft < 4)
                    {
                        notifications.Add(new Notification
                                              {
                                                  Type = "Loan",
                                                  Title = loan.DocumentTitle + " forfaller snart",
                                                  DocumentTitle = loan.DocumentTitle,
                                                  Content =
                                                      "Lånet forfaller om mindre enn " + timeLeft +
                                                      " dager. Lever eller forny " +
                                                      "lånet for å unngå å få gebyr."
                                              });
                    }
                    else if (timeLeft == 0)
                    {
                        notifications.Add(new Notification
                                              {
                                                  Type = "Loan",
                                                  Title = loan.DocumentTitle + " forfaller snart",
                                                  DocumentTitle = loan.DocumentTitle,
                                                  Content = "Lånet forfaller om mindre ett døgn. Lever eller forny " +
                                                            "lånet for å unngå å få gebyr."
                                              });
                    }
                    else if (timeLeft < 0)
                    {
                        notifications.Add(new Notification
                                              {
                                                  Type = "Fine",
                                                  Title = loan.DocumentTitle + " skulle vært levert",
                                                  DocumentTitle = loan.DocumentTitle,
                                                  Content =
                                                      "Lånet har forfalt. Ved å fornye eller levere tilbake innen forfallsdato unngår du gebyr."
                                              });

                    }

                }

            }

            if (user.Reservations != null)
            {
                foreach (var reservation in user.Reservations)
                {
                    if (reservation.HoldRequestEnd != "")
                    {
                        notifications.Add(new Notification
                                              {
                                                  Type = "Reservation",
                                                  Title = reservation.DocumentTitle + " er klar til henting.",
                                                  DocumentTitle = reservation.DocumentTitle,
                                                  Content = "Den kan hentes på " + reservation.PickupLocation + " innen " + reservation.HoldRequestEnd,
                                              });
                    }
                }
            }

            return notifications;
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
