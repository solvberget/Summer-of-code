using System;

namespace Solvberget.Core.DTOs
{
    public class LoanDto : RequestReplyDto
    {
        public string DocumentNumber { get; set; } //Matches the documentnumber used in the BSMARC posts
        public string AdminisrtativeDocumentNumber { get; set; } //Administrative number connected to the document, used by the library.
        public string ItemSequence { get; set; }
        public string Barcode { get; set; }
        public string DocumentTitle { get; set; }
        public string SubLibrary { get; set; }
        public DateTime? OriginalDueDate { get; set; }
        public string ItemStatus { get; set; }
        public DateTime? LoanDate { get; set; }
        public string LoanHour { get; set; }
        public string Material { get; set; }
        public DateTime? DueDate { get; set; }
        public DocumentDto Document { get; set; }
    }
}
