using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Solvberget.Core.Services
{
    public class HttpBodyDownloader : IStringDownloader
    {
        public async Task<string> Download(string url)
        {
            var request = HttpWebRequest.Create(url);
            var result = await request.GetResponseAsync();

            return new StreamReader(result.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        }
    }
}