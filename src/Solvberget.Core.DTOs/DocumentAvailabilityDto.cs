namespace Solvberget.Core.DTOs
{
    public class DocumentAvailabilityDto
    {
        public string Branch { get; set; }
        public int AvailableCount { get; set; }
        public int TotalCount { get; set; }
        public string Department { get; set; }
        public string Collection { get; set; }
        public string Location { get; set; }
        public string EstimatedAvailableDate { get; set; }
    }
}