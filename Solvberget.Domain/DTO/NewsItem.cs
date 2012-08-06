using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Solvberget.Domain.DTO
{

    public class NewsItem
    {
        public string Title { get; set; }
        public string DescriptionUnescaped { private get; set; }
        public DateTimeOffset PublishedDateAsDateTime { private get; set; }
        public Uri LinkAsUri { private get; set; }

        //Should be in a View-Model...?
        public string PublishedDate { get { return PublishedDateAsDateTime.ToLocalTime().ToString("dd-MM-yyyy"); } }
        public string Link { get { return LinkAsUri.AbsoluteUri; } }
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
