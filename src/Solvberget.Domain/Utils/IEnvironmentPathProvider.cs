using Solvberget.Domain.Documents;

namespace Solvberget.Domain.Utils
{
    public interface  IEnvironmentPathProvider
    {
        string GetDictionaryPath();
        string GetDictionaryIndexPath();
        string GetStopwordsPath();
        string GetImageCachePath();
        string GetSuggestionListPath();
        string GetTestDictPath();
        string GetXmlListPath();
        string GetXmlFilePath();
        string GetRulesPath();
        string GetBlogFeedPath();
        string GetOpeningInfoAsXmlPath();
        string GetContactInfoAsXmlPath();
        string ResolveUrl(string path);
        string ResolveUrl(string baseUrl, string serverPath);
        string GetPlaceHolderImagesPath();
        string GetFavoritesPath(string userId);
        string GetWebAppDocumentDetailsPath(Document document);
    }
}