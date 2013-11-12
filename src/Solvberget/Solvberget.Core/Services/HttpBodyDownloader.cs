using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class HttpBodyDownloader : IStringDownloader
    {
        private readonly IUserAuthenticationDataService _userAuthSerice;

        public HttpBodyDownloader(IUserAuthenticationDataService userAuthSerice)
        {
            _userAuthSerice = userAuthSerice;
        }

        public async Task<string> Download(string url)
        {
            return await Download(url, "GET");
        }

        public async Task<string> Download(string url, string method)
        {
            var request = HttpWebRequest.Create(url);
            if (_userAuthSerice.UserInfoRegistered())
            {
                request.Headers["Authorization"] = _userAuthSerice.GetUserId() + ":" +
                                                    _userAuthSerice.GetUserPassword();
            }
            request.Method = method;
            var result = await request.GetResponseAsync();

            return new StreamReader(result.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        }

        public async Task<string> Download(string url, string method, string userId, string userPin)
        {
            var request = HttpWebRequest.Create(url);
            if (_userAuthSerice.UserInfoRegistered())
            {
                request.Headers["Authorization"] = userId + ":" + userPin;
            }
            request.Method = method;
            var result = await request.GetResponseAsync();

            return new StreamReader(result.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        }
    }
}