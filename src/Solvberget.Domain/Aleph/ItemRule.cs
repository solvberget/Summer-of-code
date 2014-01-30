namespace Solvberget.Domain.Aleph
{
    public class ItemRule
    {
        public string Library { get; set; }
        public string ItemStatus { get; set; }
        public string ProcessStatusCode { get; set; }
        public string ProcessStatusText { get; set; }
        public bool CanBorrow { get; set; }
        public bool CanRenew { get; set; }
        public bool CanReserve { get; set; }
        public bool ShownOnWebCataloge { get; set; }
    }
}
