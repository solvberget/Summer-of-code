
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
            Link = "http://www.stavanger-kulturhus.no/kontakt_oss";
        }

        public void FillProperties()
        {
            var html = GetHtml();
            var node = GetDiv(html, "attribute-long");
            //var attributes = StripHtmlTags("attribute-long");

            var h3List   = GetValue(node, "h3");
            var pList = GetValue(node, "p");
            var ullist = GetValue(node, "ul");
            var iteratorList = h3List;
            
            
            //This if to sort the current HTML
            if (h3List.Count < ullist.Count)
            {
                iteratorList = ullist;
                for (var i = 0; i < iteratorList.Count; i++)
                {
                    var pH3 = pList[i];
                    if(pH3 != "")
                    {
                        h3List.Insert(i, pH3);
                        pList.RemoveAt(i);
                    }
                }
            }
            
            for (var i = 0; i < iteratorList.Count; i++)
            {
                var contactInformation = new ContactInformation();
                contactInformation.Department = h3List[i];
            }


        }


    }
}
