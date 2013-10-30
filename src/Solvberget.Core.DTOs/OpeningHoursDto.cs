using System.Collections.Generic;

namespace Solvberget.Core.DTOs
{
    public class OpeningHoursDto
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Dictionary<string, string> Hours { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string Url { get; set; }
        public string UrlText { get; set; }
    }
}
