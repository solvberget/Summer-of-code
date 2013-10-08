
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace Solvberget.Domain.DTO
{
    public sealed class ContactWebPage : WebPage
    {
        public List<ContactInformation> ContactInformationList { get; set; }
     
        public ContactWebPage()
        {
            Link = Properties.Settings.Default.ContactWebPage;
        }

        public void FillProperties()
        {

            ContactInformationList = new List<ContactInformation>();
            var html = GetHtml();
            var node = GetDiv(html, "attribute-long");
            //var attributes = StripHtmlTags("attribute-long");

            var h3List   = GetValue(node, "h3");
            var pList = GetValue(node, "p");
            var ullist = GetValue(node, "ul");
            var iteratorList = ullist;
            var email = "";
            //var emailString = "";
            
            //This is to sort the current HTML
            if (h3List.Count < ullist.Count)
            {
                h3List.Insert(5, pList[5]);
                pList.RemoveAt(5);
            }

            for (var i = 0; i < iteratorList.Count; i++)
            {
                ullist[i] = ullist[i] + "\n";
            }

            for (var i = 0; i < iteratorList.Count; i++)
            {
                //Remove multipe whitespaces
                var options = RegexOptions.None;
                var emailRegex = new Regex(@"([.a-z0-9-]+)*@(stavanger-kulturhus.no)", options);
                email = emailRegex.Match(iteratorList[i]).Value;

                var emailStringRegex = new Regex(@"(E-postadresse)", options);
                ullist[i] = emailStringRegex.Replace(iteratorList[i], "E-post");

                var contactInformation = new ContactInformation
                                             {
                                                 InformationTitle = h3List[i], 
                                                 InformationValue = ullist[i],
                                                 Email = email
                                             };
                //contactInformation.FillProperties();
                ContactInformationList.Add(contactInformation);
            }
        }
    }
}
