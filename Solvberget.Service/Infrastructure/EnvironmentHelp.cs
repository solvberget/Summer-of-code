using System.IO;
using System.Web.Hosting;

namespace Solvberget.Service.Infrastructure
{
    public class EnvironmentHelper
    {
        private static readonly string ApplicationAppDataPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"bin\App_Data");
        
 

        public static string GetDictionaryPath()
        {
            return Path.Combine(ApplicationAppDataPath, @"ordlister\ord_bm.txt");    
        }

        public static string GetDictionaryIndexPath()
        {
            return Path.Combine(ApplicationAppDataPath, "ordlister_index");
        }

        public static string GetStopwordsPath()
        {
            return Path.Combine(ApplicationAppDataPath, @"ordlister\stopwords.txt");
        }


        public static string GetSuggestionListPath()
        {
            return Path.Combine(ApplicationAppDataPath, @"ordlister\ord_forslag.txt");
        }
        public static string GetTestDictPath()
        {
            return Path.Combine(ApplicationAppDataPath, @"ordlister\ord_test.txt");
        }
    }
}