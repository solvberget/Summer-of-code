﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Solvberget.Core.Properties;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IStringDownloader _downloader;

        public NewsService(IStringDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<IEnumerable<NewsItem>> GetNews()
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_News);
                var results = JsonConvert.DeserializeObject<JContainer>(response);

                return from r in results
                                       select new NewsItem
                                       {
                                           Title = r["Title"].Value<string>(),
                                           DescriptionUnescaped = r["Description"].Value<string>(),
                                           Link = new Uri(r["Link"].Value<string>()),
                                           PublishedDateAsDateTime = new DateTimeOffset(r["PublishedDateAsDateTime"]["DateTime"].Value<DateTime>())
                                       };
            }
            catch (Exception)
            {
                return new List<NewsItem>
                {
                    new NewsItem
                    {
                        Title = "Feil ved lasting",
                        DescriptionUnescaped = "Kunne desverre ikke finne noen nyheter. Prøv igjen senere.",
                        PublishedDateAsDateTime = DateTime.Now,
                        Link = new Uri("http://solvberget.no")
                    }
                };
            }
        }
    }
}