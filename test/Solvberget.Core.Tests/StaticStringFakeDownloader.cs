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
    }
}