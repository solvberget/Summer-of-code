using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IStringDownloader _downloader;

        public NewsService(IStringDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<IEnumerable<NewsStoryDto>> GetNews()
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_News);
                return JsonConvert.DeserializeObject<IList<NewsStoryDto>>(response);
            }
            catch (Exception)
            {
                return new List<NewsStoryDto>
                {
                    new NewsStoryDto
                    {
                        Title = "Feil ved lasting",
                        Ingress = "Kunne desverre ikke finne noen nyheter. Prøv igjen senere.",
                        Published = DateTime.Now,
                        Link = new Uri("http://solvberget.no")
                    }
                };
            }
        }
    }
}