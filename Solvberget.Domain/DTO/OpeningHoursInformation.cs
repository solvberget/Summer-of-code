using System.Collections.Generic;

namespace Solvberget.Domain.DTO
{
    public class OpeningHoursInformation
    {

        public string Location { get; set; }
        public string Contact { get; set; }
        public string ExternalLink  { get; set; }
        public Dictionary<string, string> WeekDayHours { get; set; }
      
      
    }
}
