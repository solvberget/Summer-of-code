using System.IO;

using Solvberget.Domain.Abstract;

namespace Solvberget.Domain.Implementation
{
    public class EnvironmentPathProvider : IEnvironmentPathProvider
    {
        private readonly string _applicationAppDataPath;

        private readonly string _applicationContentDataPath;

        public EnvironmentPathProvider(IRootPathProvider rootPathProvider)
        {
            var rootPath = rootPathProvider.GetRootPath();

            _applicationAppDataPath = Path.Combine(rootPath, @"bin\App_Data");
            _applicationContentDataPath = Path.Combine(rootPath, @"Content");
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
            return Path.Combine(_applicationContentDataPath, @"cacheImages\");
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
    }
}