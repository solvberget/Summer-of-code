using System;
using System.Collections.Generic;

namespace Solvberget.Domain.DTO
{
    public class AvailabilityInformation
    {
        public static readonly IEnumerable<string> BranchesToHandle = new List<string> { "Hovedbibl.", "Madla" };
        public static readonly int AveragePickupTimeInDays = 4;

        public string Branch { get; set; }
        public IEnumerable<string> Department { get; set; }
        public string PlacementCode { get; set; }
        public int TotalCount { get; set; }
        public int AvailableCount { get; set; }
        private DateTime? EarliestAvailableDate { get; set; }
        public string EarliestAvailableDateFormatted { get; set; }
        public string RecKeyIfAvailable { get; set; } //For later use with reservartion
    }
}
