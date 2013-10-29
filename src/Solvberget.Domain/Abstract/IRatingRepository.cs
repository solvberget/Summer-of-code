namespace Solvberget.Domain.Abstract
{
    public class DocumentRating
    {
        public string SourceUrl { get; set; }
        public double Score { get; set; }
        public double MaxScore { get; set; }
        public string Source { get; set; }
    }

    public interface IRatingRepository
    {
        DocumentRating GetDocumentRating(string id);
    }
}