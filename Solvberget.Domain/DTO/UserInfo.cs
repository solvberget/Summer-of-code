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


            xElementRecord = xElement.Element("fine");
            if (xElementRecord == null) return;
            

            var fines = new List<Fine>();

            var varfields = xElement.Elements("fine").ToList();
            foreach (var varfield in varfields)
            {

                var temp = varfield.Elements().ToList();

                var xElementField = xElementRecord.Element("z31");
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


                //Get the title of the document mentioned in the fine based on the docnumber
                string docId = "";
                string docTitle = "";
                string date = "";

                date = GetFormattedDate(GetXmlValue(temp.ElementAt(0), "z31-date"));

                var status = GetXmlValue(temp.ElementAt(0), "z31-status");

                if (status == "Not paid by/credited to patron")
                    status = "Ikke betalt ";

                if(temp.Count > 1)
                {
                    //docId = temp.ElementAt(1).ToString().Substring(25, 6);



                    var element = temp.ElementAt(1).ToString();

                    var regExp = new Regex(@"(-doc-number>)(\d+)(</)");

                    docId = regExp.Match(element).Groups[2].ToString();

                    regExp = new Regex(@"(-title>)(\D+)(</)");
                    docTitle = regExp.Match(element).Groups[2].ToString();

                }

                var description = GetXmlValue(temp.ElementAt(0), "z31-type");
                string descriptionLookupValue = null;
                if (description != null)
                    TypeDictionary.TryGetValue(description, out descriptionLookupValue);

                var fine = new Fine()
                               {
                                   Date = date,
                                   Status = status,
                                   CreditDebit = Convert.ToChar(GetXmlValue(temp.ElementAt(0), "z31-credit-debit")),
                                   Sum = sum,
                                   Description = descriptionLookupValue ?? description,
                                   DocumentNumber = docId,
                                   DocumentTitle = docTitle
                               };
                fines.Add(fine);
            }

            Fines = fines;
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


        protected static readonly Dictionary<string, string> TypeDictionary = new Dictionary<string, string>
                                {
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
