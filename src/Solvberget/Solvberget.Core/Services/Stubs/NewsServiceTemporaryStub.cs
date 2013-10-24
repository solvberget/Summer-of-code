using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Stubs
{
    public class NewsServiceTemporaryStub : INewsService
    {
        public async Task<IEnumerable<NewsItem>> GetNews()
        {
            await TaskEx.Delay(1200);
            return new List<NewsItem>
            {
                new NewsItem {DescriptionUnescaped = "Ny forskning! Bacon er godt sier nyere studier!.", Title = "Bacon <3", Link = new Uri("http://online.ntnu.no"), PublishedDateAsDateTime = DateTime.Now},
                new NewsItem {DescriptionUnescaped = "Folk som låner bøker på biblioteket mindre innblandet i kriminalitet enn gjennomsnittet!", Title = "Bøker gjør deg lovlydig!", Link = new Uri("http://capgemini.no"), PublishedDateAsDateTime = DateTime.Now - TimeSpan.FromDays(1)},
                new NewsItem {DescriptionUnescaped = "Ny forskning! Bacon er godt sier nyere studier!.", Title = "Bacon <3", Link = new Uri("http://online.ntnu.no"), PublishedDateAsDateTime = DateTime.Now},
                new NewsItem {DescriptionUnescaped = "Folk som låner bøker på biblioteket mindre innblandet i kriminalitet enn gjennomsnittet!", Title = "Bøker gjør deg lovlydig!", Link = new Uri("http://capgemini.no"), PublishedDateAsDateTime = DateTime.Now - TimeSpan.FromDays(1)},
                new NewsItem {DescriptionUnescaped = "Ny forskning! Bacon er godt sier nyere studier!.", Title = "Bacon <3", Link = new Uri("http://online.ntnu.no"), PublishedDateAsDateTime = DateTime.Now},
                new NewsItem {DescriptionUnescaped = "Folk som låner bøker på biblioteket mindre innblandet i kriminalitet enn gjennomsnittet!", Title = "Bøker gjør deg lovlydig!", Link = new Uri("http://capgemini.no"), PublishedDateAsDateTime = DateTime.Now - TimeSpan.FromDays(1)},
                new NewsItem {DescriptionUnescaped = "Ny forskning! Bacon er godt sier nyere studier!.", Title = "Bacon <3", Link = new Uri("http://online.ntnu.no"), PublishedDateAsDateTime = DateTime.Now},
                new NewsItem {DescriptionUnescaped = "Folk som låner bøker på biblioteket mindre innblandet i kriminalitet enn gjennomsnittet!", Title = "Bøker gjør deg lovlydig!", Link = new Uri("http://capgemini.no"), PublishedDateAsDateTime = DateTime.Now - TimeSpan.FromDays(1)}
            };
        }
    }
}