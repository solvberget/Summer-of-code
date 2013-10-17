namespace Solvberget.Domain.Abstract
{
    public interface IImageRepository
    {
        string GetDocumentImage(string id);
        string GetDocumentThumbnailImage(string id, string size);
    }
}