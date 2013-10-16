namespace Solvberget.Domain.Abstract
{
    public interface IEnvironmentPathProvider
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
    }
}