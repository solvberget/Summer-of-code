using System;
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
            var ohi = new OpeningHoursInformation {LocationOrDayOfWeekToTime = new Dictionary<string, string>()};
            ohi.FillProperties(xml);
            return ohi;
        }

        private void FillProperties(XContainer xml)
        {
            if (xml == null) return;

            var title = xml.Element("title");
            var subTitle = xml.Element("subtitle");
            var location = xml.Element("location");
            var phone = xml.Element("phone");
            var url = xml.Element("url");
            var urlText = xml.Element("urlText");

            if (title != null) Title = string.IsNullOrEmpty(title.Value) ? null : title.Value;
            if (subTitle != null) SubTitle = string.IsNullOrEmpty(subTitle.Value) ? null : subTitle.Value;
            if (location != null) Location = string.IsNullOrEmpty(location.Value) ? null : location.Value;
            if (phone != null) Phone = string.IsNullOrEmpty(phone.Value) ? null : phone.Value;
            if (url != null) Url = string.IsNullOrEmpty(url.Value) ? null : url.Value;
            if (urlText != null) UrlText = string.IsNullOrEmpty(urlText.Value) ? null : urlText.Value;

            var keyValueList = xml.Element("departmentOrDayOfWeekToTimeList");
            if (keyValueList == null) return;
            var items = keyValueList.Descendants("item");
            foreach (var item in items)
            {
                var key = item.Attribute("key");
                if (key == null) continue;
                var value = item.Element("value");
                if (value != null) LocationOrDayOfWeekToTime.Add(key.Value, value.Value ?? string.Empty);
            }
        }

    }

}

