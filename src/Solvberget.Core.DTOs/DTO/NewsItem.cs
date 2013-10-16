using System;
using System.Text.RegularExpressions;

namespace Solvberget.Domain.DTO
{
    public class NewsItem
    {
        public string Title { get; set; }
        public Uri Link { get; set; }
        public string DescriptionUnescaped { private get; set; }
        public DateTimeOffset PublishedDateAsDateTime { private get; set; }

        public string PublishedDate { get { return "Publisert: " + PublishedDateAsDateTime.ToLocalTime().ToString("dd.MM.yyyy"); } }
        public string Description
        {
            get
            {
                var desc = Regex.Replace(DescriptionUnescaped, "<(.|\n)*?>", "");
                return Regex.Replace(desc, "\\n", "");
            }
        }
    }
}
