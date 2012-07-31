using System.Collections.Generic;

namespace Solvberget.Domain.DTO
{
    public class OpeningHoursInformation : Information
    {
        public string Title { get; set; }
        public Dictionary<string, string> WeekDayHours { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string Contact { get; set; }
        public string ExternalLink { get; set; }

    }

}

