namespace Solvberget.Domain.Abstract
{
    public interface IRatingRepository
    {
        DocumentRating GetDocumentRating(string id);
    }
}