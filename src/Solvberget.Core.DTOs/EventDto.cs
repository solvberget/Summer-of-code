using System;

namespace Solvberget.Core.DTOs
{
    public class EventDto
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; }

        public double TicketPrice { get; set; }
    }
}