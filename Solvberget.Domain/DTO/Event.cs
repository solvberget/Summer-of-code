using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{

    public class Event
    {
        private static readonly CultureInfo NbNoCultureInfo = new CultureInfo("nb-NO");
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }
        public string DateAsString { private get; set; }
        public string DateFormatted
        {
            get
            {
                if (!String.IsNullOrEmpty(DateAsString))
                {
                    var dateAsDateTime = DateTime.Parse(DateAsString);
                    return dateAsDateTime.ToString("d. MMMM yyyy", NbNoCultureInfo);
                }
                else
                {
                    return "Ukjent dato";
                }
            }
        }
        public string Month
        {
            get
            {
                if (!String.IsNullOrEmpty(DateAsString))
                {
                    var dateAsDateTime = DateTime.Parse(DateAsString);
                    var month = dateAsDateTime.ToString("MMMM", NbNoCultureInfo);
                    return char.ToUpper(month[0]) + month.Substring(1);
                }
                else
                {
                    return "Ukjent måned";
                }
            }
        }
        public string Start { get; set; }
        public string Stop { get; set; }
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
    }
}
