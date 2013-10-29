namespace Solvberget.Domain.DTO
{
    public class Reservation
    {
        public string DocumentNumber { get; set; }
        public string DocumentTitle { get; set;  }
        public string PickupLocation { get; set; }
        public string HoldRequestFrom { get; set; }
        public string HoldRequestTo { get; set; }
        public string CancellationSequence { get; set; }
        public string ItemSeq { get; set; }
        public string ItemDocumentNumber { get; set; }
        public string Status { get; set; }
        public string HoldRequestEnd { get; set; }
    }
}
