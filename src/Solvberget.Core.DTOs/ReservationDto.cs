using System;

namespace Solvberget.Core.DTOs
{
    public class ReservationDto
    {
        public string DocumentNumber { get; set; }
        public string DocumentTitle { get; set; }
        public string PickupLocation { get; set; }
        public DateTime? HoldRequestFrom { get; set; }
        public DateTime? HoldRequestTo { get; set; }
        public string CancellationSequence { get; set; }
        public string ItemSeq { get; set; }
        public string ItemDocumentNumber { get; set; }
        public string Status { get; set; }
        public DateTime? HoldRequestEnd { get; set; }
    }
}
