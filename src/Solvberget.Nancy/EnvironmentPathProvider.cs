using System.IO;
using Nancy;
using Solvberget.Domain.Utils;

namespace Solvberget.Nancy
{
    public class EnvironmentPathProvider : IEnvironmentPathProvider
    {
        private readonly string _applicationAppDataPath;

        private readonly string _applicationContentDataPath;
        private readonly string _rootPath;

        private readonly string _rootUrl;

        public EnvironmentPathProvider(IRootPathProvider rootPathProvider)
        {
            _rootPath = rootPathProvider.GetRootPath();
            _rootUrl = "http://localhost:39465/"; // todo: 
         
            _applicationAppDataPath = Path.Combine(_rootPath, @"Data");
            _applicationContentDataPath = Path.Combine(_rootPath, @"Content");
        }

        public string GetDictionaryPath()
        {
            return Path.Combine(_applicationAppDataPath, @"ordlister\ord_bm.txt");
        }

        public string GetDictionaryIndexPath()
        {
            return Path.Combine(_applicationAppDataPath, "ordlister_index");
        }

        public string GetStopwordsPath()
        {
            return Path.Combine(_applicationAppDataPath, @"ordlister\stopwords.txt");
        }

        public string GetImageCachePath()
        {
            return Path.Combine(_applicationContentDataPath, @"CachedImages\");
        }

        public string GetSuggestionListPath()
        {
            return Path.Combine(_applicationAppDataPath, @"ordlister\ord_forslag.txt");
        }

        public string GetTestDictPath()
        {
            return Path.Combine(_applicationAppDataPath, @"ordlister\ord_test.txt");
        }

        public string GetXmlListPath()
        {
            return Path.Combine(_applicationAppDataPath, @"librarylists\static");
        }

        public string GetXmlFilePath()
        {
            return Path.Combine(_applicationAppDataPath, @"librarylists\dynamic");
        }

        public string GetRulesPath()
        {
            return Path.Combine(_applicationAppDataPath, @"rules\");
        }

        public string GetBlogFeedPath()
        {
            return Path.Combine(_applicationAppDataPath, @"blogs\");
        }

        public string GetOpeningInfoAsXmlPath()
        {
            return Path.Combine(_applicationAppDataPath, @"info\opening.xml");
        }

        public string GetContactInfoAsXmlPath()
        {
            return Path.Combine(_applicationAppDataPath, @"info\contact.xml");
        }

        public string ResolveUrl(string serverPath)
        {
            var relative = serverPath.ToLowerInvariant().Replace(_rootPath.ToLowerInvariant(), "").Replace('\\','/');
            var absolute = Path.Combine(_rootUrl, relative);
            return absolute;
        }

        public string ResolveUrl(string baseUrl, string serverPath)
        {
            var relative = serverPath.ToLowerInvariant().Replace(_rootPath.ToLowerInvariant(), "").Replace('\\', '/');
            var absolute = Path.Combine(baseUrl, relative);
            return absolute;
        }
    }
}