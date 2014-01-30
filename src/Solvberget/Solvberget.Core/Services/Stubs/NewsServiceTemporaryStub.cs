using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services.Stubs
{
    public class NewsServiceTemporaryStub : INewsService
    {
        public async Task<IEnumerable<NewsStoryDto>> GetNews()
        {
            await TaskEx.Delay(1200);
            return new List<NewsStoryDto>
            {
                new NewsStoryDto {Ingress = "Ny forskning! Bacon er godt sier nyere studier!.", Title = "Bacon <3", Link = new Uri("http://online.ntnu.no"), Published = DateTime.Now},
                new NewsStoryDto {Ingress = "Folk som låner bøker på biblioteket mindre innblandet i kriminalitet enn gjennomsnittet!", Title = "Bøker gjør deg lovlydig!", Link = new Uri("http://capgemini.no"), Published = DateTime.Now - TimeSpan.FromDays(1)},
                new NewsStoryDto {Ingress = "Ny forskning! Bacon er godt sier nyere studier!.", Title = "Bacon <3", Link = new Uri("http://online.ntnu.no"), Published = DateTime.Now},
                new NewsStoryDto {Ingress = "Folk som låner bøker på biblioteket mindre innblandet i kriminalitet enn gjennomsnittet!", Title = "Bøker gjør deg lovlydig!", Link = new Uri("http://capgemini.no"), Published = DateTime.Now - TimeSpan.FromDays(1)},
                new NewsStoryDto {Ingress = "Ny forskning! Bacon er godt sier nyere studier!.", Title = "Bacon <3", Link = new Uri("http://online.ntnu.no"), Published = DateTime.Now},
                new NewsStoryDto {Ingress = "Folk som låner bøker på biblioteket mindre innblandet i kriminalitet enn gjennomsnittet!", Title = "Bøker gjør deg lovlydig!", Link = new Uri("http://capgemini.no"), Published = DateTime.Now - TimeSpan.FromDays(1)},
                new NewsStoryDto {Ingress = "Ny forskning! Bacon er godt sier nyere studier!.", Title = "Bacon <3", Link = new Uri("http://online.ntnu.no"), Published = DateTime.Now},
                new NewsStoryDto {Ingress = "Folk som låner bøker på biblioteket mindre innblandet i kriminalitet enn gjennomsnittet!", Title = "Bøker gjør deg lovlydig!", Link = new Uri("http://capgemini.no"), Published = DateTime.Now - TimeSpan.FromDays(1)}
            };
        }
    }
}