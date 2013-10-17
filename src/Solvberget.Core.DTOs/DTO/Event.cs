using System;
using System.Globalization;

namespace Solvberget.Domain.DTO
{

    public class Event
    {
        private static readonly CultureInfo NbNoCultureInfo = new CultureInfo("nb-NO");
        private const string SingleEventAsICalBase = "http://www.linticket.no/iCal/Sølvberget/index.php3?Arr=";

        public string Id { get; set; }
        public string Name { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }
        public string DateAsString { private get; set; }
        public string DateFormatted
        {
            get
            {
                if (String.IsNullOrEmpty(DateAsString))
                    return "Ukjent dato";

                var dateAsDateTime = DateTime.Parse(DateAsString);
                return dateAsDateTime.ToString("d. MMMM yyyy", NbNoCultureInfo);
            }
        }
        public string Month
        {
            get
            {
                if (String.IsNullOrEmpty(DateAsString))
                    return "Ukjent måned";
                
                var dateAsDateTime = DateTime.Parse(DateAsString);
                var month = dateAsDateTime.ToString("MMMM", NbNoCultureInfo);
                return char.ToUpper(month[0]) + month.Substring(1);
            }
        }
        public string Group { get { return (DateTime.Parse(DateAsString).Month - 1).ToString(); } }
        public string Start { get; set; }
        public string StartFormatted { get { return TrimTime(Start); } }
        public string Stop { get; set; }
        public string StopFormatted { get { return TrimTime(Stop); } }
        public string DateAndTime
        {
            get
            {
                if (String.IsNullOrEmpty(StartFormatted) || String.IsNullOrEmpty(StopFormatted))
                    return DateFormatted;
            
                return DateFormatted + " " + StartFormatted + "-" + StopFormatted;
            
            }
        }
        public string Link { get; set; }
        public string PictureUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string Description { get; set; }
        public string Teaser { get; set; }
        public string TypeId { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string TypeName { get; set; }
        public string TicketPrice { get; set; }
        public string ICalLink
        {
            get
            {
                return SingleEventAsICalBase + Id;
            }
        }

        private static string TrimTime(string time)
        {
            if (time == null)
                return string.Empty;
            if (time.Equals("Ukjent") || time.Equals("ukjent"))
                return string.Empty;
            var t = time.Trim();
            if (String.IsNullOrEmpty(t))
                return string.Empty;
            return t.Length > 7 ? t.Substring(0, 5) : t;
        }
    }
}
