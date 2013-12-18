using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Properties;

namespace Solvberget.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly DtoDownloader _downloader;

        public NewsService(DtoDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<IEnumerable<NewsStoryDto>> GetNews()
        {
            var result = await _downloader.DownloadList<NewsStoryDto>(Resources.ServiceUrl + Resources.ServiceUrl_News);
            return result.Results;
        }
    }
}