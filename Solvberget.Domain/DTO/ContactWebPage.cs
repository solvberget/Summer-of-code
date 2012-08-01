
using System;
using System.Collections.Generic;
using System.Linq;
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
                var contactInformation = new ContactInformation {InformationTitle = h3List[i], InformationValue = ullist[i]};
                //contactInformation.FillProperties();
                ContactInformationList.Add(contactInformation);
            }


        }


    }
}
