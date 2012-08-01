using System.Collections.Generic;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class OpeningHoursWebPage : WebPage
    {

        public List<OpeningHoursInformation> OpeningHoursInformationList { get; set; }
     
        public OpeningHoursWebPage()
        {
            Link = Properties.Settings.Default.OpeningHoursWebpage;
        }
        
        public void FillProperties()
        {
            OpeningHoursInformationList = new List<OpeningHoursInformation>();

            var html = GetHtml();
            var node = GetDiv(html, "attribute-long");
            //var attributes = StripHtmlTags("attribute-long");

            var h3List = GetValue(node, "h3");
            var pList = GetValue(node, "p");
            var tableList = GetValue(node, "table");
            var iteratorList = h3List;
           
            //This is to sort the current HTML
            if (h3List.Count < tableList.Count)
            {
                h3List.Insert(0, pList[0]);
                pList.RemoveAt(0);
            }

            for (var i = 0; i < iteratorList.Count; i++)
            {
                tableList[i] = tableList[i] + "\n";
            }
            
            for (var i = 0; i < iteratorList.Count; i++)
            {
                var openingHoursInformation = new OpeningHoursInformation { InformationTitle = h3List[i], InformationValue = tableList[i] };
                //openingHoursInformation.FillProperties();
                OpeningHoursInformationList.Add(openingHoursInformation);
            }
        }
    }
}



