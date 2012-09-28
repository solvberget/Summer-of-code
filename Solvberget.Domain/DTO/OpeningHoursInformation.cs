using System.Collections.Generic;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class OpeningHoursInformation : Information
    {
        
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Dictionary<string, string> LocationOrDayOfWeekToTime { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string Url { get; set; }
        public string UrlText { get; set; }

        public static OpeningHoursInformation GenerateFromXml(XElement xml)
        {
            var ohi = new OpeningHoursInformation();
            ohi.FillProperties(xml);
            return ohi;    
        }

        private void FillProperties(XElement xml)
        {
            
            var title = xml.Element("title");
            var subTitle = xml.Element("subtitle");
            var location = xml.Element("title");
            var phone = xml.Element("title");
            var url = xml.Element("title");
            var urlText = xml.Element("title");

            if (title != null) Title = title.Value;
            if (subTitle != null) SubTitle = subTitle.Value;
            if (location != null) Location = location.Value;
            if (phone != null) Phone = phone.Value;
            if (url != null) Url = url.Value;
            if (UrlText != null) UrlText = urlText.Value;

            //Key-value

        }

    }

}

