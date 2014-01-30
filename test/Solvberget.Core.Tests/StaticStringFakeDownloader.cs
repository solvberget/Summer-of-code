using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.Services;

namespace Solvberget.Core.Tests
{
    internal class StaticStringFakeDownloader : IStringDownloader
    {
        private readonly string _toBeReturned;

        public StaticStringFakeDownloader(string toBeReturned)
        {
            _toBeReturned = toBeReturned;
        }

        public async Task<string> Download(string url)
        {
            await TaskEx.Delay(10);
            return _toBeReturned;
        }

        public async Task<string> Download(string url, string method)
        {
            return await Download(url);
        }

        public async Task<string> PostForm(string url, Dictionary<string, string> formData)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> Download(string url, string method, string userId, string userPin)
        {
            throw new System.NotImplementedException();
        }
    }
}