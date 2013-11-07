using System;

namespace Solvberget.Core.DTOs
{
    public class EventDto
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public string Location { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public double TicketPrice { get; set; }
        public string TicketUrl { get; set; }
    }
}