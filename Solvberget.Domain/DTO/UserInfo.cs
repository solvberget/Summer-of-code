﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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



            DateOfBirth = GetFormattedDateOfBirth(GetXmlValue(xElementRecord, "z303-birth-date"));




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

        private static string GetFormattedDateOfBirth(string dateOfBirth)
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
    }

}