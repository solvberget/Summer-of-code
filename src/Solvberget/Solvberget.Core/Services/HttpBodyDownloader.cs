using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task<string> PostForm(string url, Dictionary<string, string> formData)
        {
            var request = HttpWebRequest.Create(url);
            if (_userAuthSerice.UserInfoRegistered())
            {
                request.Headers["Authorization"] = _userAuthSerice.GetUserId() + ":" +
                                                    _userAuthSerice.GetUserPassword();
            }
            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded";

            var parameters = string.Join("&", formData.Select((k, v) => string.Format("{0}={1}", k, v)));
            var byteContent = Encoding.UTF8.GetBytes(parameters);
            
            var contentStream = await request.GetRequestStreamAsync();
            contentStream.Write(byteContent, 0, byteContent.Length);

            var result = await request.GetResponseAsync();

            return new StreamReader(result.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        }
    }
}