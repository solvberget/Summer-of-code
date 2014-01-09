using System;
using System.IO;
using System.Web.Configuration;
using Nancy;
using Nancy.Helpers;
using Solvberget.Domain.Documents;
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

            var dataPath = WebConfigurationManager.AppSettings["DataPath"];
            if (String.IsNullOrEmpty(dataPath)) dataPath = _rootPath;

            _applicationAppDataPath = Path.Combine(dataPath, @"Data");
            _applicationContentDataPath = Path.Combine(rootPathProvider.GetRootPath(), @"Content");
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

        public string GetPlaceHolderImagesPath()
        {
            return Path.Combine(_applicationAppDataPath, @"placeholder_images\");
        }

        public string GetFavoritesPath(string userId)
        {
            var favPath = Path.Combine(_applicationAppDataPath, @"favorites\");

            if(!Directory.Exists(favPath)) Directory.CreateDirectory(favPath);

            return Path.Combine(favPath, userId);
        }

        public string GetWebAppUrl()
        {
            return "http://app.solvberget.no";
        }

        public string GetWebAppDocumentDetailsPath(Document document)
        {
            return "http://www.solvberget.no"; // until web app deployed
            var docUrl = Path.Combine(GetWebAppUrl(), GetWebType(document.DocType), document.DocumentNumber, HttpUtility.UrlEncode(document.Title));
            return docUrl;
        }

        private string GetWebType(string docType)
        {
            switch (docType)
            {
                case "Book" :
                    return "bok";
                case "Cd":
                    return "cd";
                case "Film":
                    return "film";
                case "AudioBook":
                    return "lydbok";
                case "SheetMusic":
                    return "noter";
                case "Game":
                    return "spill";
                case "OtherJournal":
                    return "journal";
                default:
                    return "annet";
            }
        }

        public string GetSlideConfigurationPath()
        {
            return Path.Combine(_applicationAppDataPath, @"infoscreen\screen_configuration.json");
        }

        public string GetEventsPath()
        {
            return Path.Combine(_applicationAppDataPath, @"events\events.xml");
        }
    }
}