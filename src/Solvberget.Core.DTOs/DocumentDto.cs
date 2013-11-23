namespace Solvberget.Core.DTOs
{
    public class DocumentDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Language { get; set; }
        public string[] Languages { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string MainContributor { get; set; }
        public string Publisher { get; set; }
        public DocumentAvailabilityDto[] Availability { get; set; }
        public bool? IsFavorite { get; set; }
        public bool? IsReserved { get; set; }
    }
}