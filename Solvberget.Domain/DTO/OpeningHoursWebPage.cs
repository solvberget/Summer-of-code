using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class OpeningHoursWebPage : WebPage
    {


       

        public OpeningHoursWebPage()
        {
            Link = "http://www.stavanger-kulturhus.no/aapningstider_paa_soelvberget";
        }


        public void FillProperties()
        {
            var html = GetHtml();
            var node = GetDiv(html, "attribute-long");
            //var attributes = StripHtmlTags("attribute-long");

            var h3List = GetValue(node, "h3");

            var pList = GetValue(node, "p");
            h3List.Insert(0,pList[0]);
            pList.RemoveAt(0);
            var temp = new OpeningHoursInformation();

        }


    }
}
