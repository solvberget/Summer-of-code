namespace Solvberget.Domain.Documents.Images
{
    public interface IImageRepository
    {
        string GetDocumentImage(string id);
        string GetDocumentThumbnailImage(string id, string size);
    }
}