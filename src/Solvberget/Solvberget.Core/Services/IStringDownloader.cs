using System.Threading.Tasks;

namespace Solvberget.Core.Services
{
    public interface IStringDownloader
    {
        Task<string> Download(string url);
    }
}