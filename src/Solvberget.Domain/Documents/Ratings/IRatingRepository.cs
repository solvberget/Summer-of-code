namespace Solvberget.Domain.Documents.Ratings
{
    public interface IRatingRepository
    {
        DocumentRating GetDocumentRating(string id);
    }
}