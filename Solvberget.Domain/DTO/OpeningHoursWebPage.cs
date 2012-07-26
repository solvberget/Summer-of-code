using System.Collections.Generic;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class OpeningHoursWebPage : WebPage
    {

        public List<Information> OpeningHoursInformationList { get; set; }


        public OpeningHoursWebPage()
        {
            Link = "http://www.stavanger-kulturhus.no/aapningstider_paa_soelvberget";
        }


        public void FillProperties()
        {
            OpeningHoursInformationList = new List<Information>();
            var html = GetHtml();
            var node = GetDiv(html, "attribute-long");
            //var attributes = StripHtmlTags("attribute-long");

            var h3List = GetValue(node, "h3");

            var tableList = GetValue(node, "table");
            var iteratorList = h3List;
            for (var i = 0; i < iteratorList.Count; i++)
            {
                var openingHoursInformation = new Information { InformationTitle = h3List[i], InformationValue = tableList[i]+"," };

                OpeningHoursInformationList.Add(openingHoursInformation);
            }


        }



    }
}



