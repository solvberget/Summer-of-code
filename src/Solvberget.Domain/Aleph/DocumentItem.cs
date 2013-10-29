using System;

namespace Solvberget.Domain.DTO
{
    public class DocumentItem
    {
        public string ItemKey { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string ItemStatus { get; set; }
        public string PlacementCode { get; set; }
        public bool OnHold { get; set; }
        public bool InTransit { get; set; }
        public string LoanStatus { get; set; }
        public string ItemAdmKey { get; set; }
        public string ItemKeySequence { get; set; }
        public string ItemProcessStatusText { get; set; }
        public string Barcode { get; set; }
        public bool IsReservable { get; set; }
        public int NoRequests { get; set; }

        public DateTime? LoanDueDate { get; set; }
    }
}
