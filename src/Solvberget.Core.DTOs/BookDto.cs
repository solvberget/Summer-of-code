namespace Solvberget.Core.DTOs
{
    public class BookDto : DocumentDto
    {
        public string Classification { get; set; }

        public string AuthorName { get; set; }

        public string Language { get; set; }

        public string Publisher { get; set; }

        public int PublicationYear { get; set; }

        public BookSeriesDto Series { get; set; }
    }
}