using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solvberget.Core.Services
{
    public interface IStringDownloader
    {
        Task<string> Download(string url);
        Task<string> Download(string url, string method);
        Task<string> PostForm(string url, Dictionary<string, string> formData);
    }
}