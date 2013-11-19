using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Properties;

namespace Solvberget.Core.Services
{
    public class EventService : IEventService
    {
        private readonly IStringDownloader _downloader;

        public EventService(IStringDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<IList<EventDto>> GetList()
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Events);
                return JsonConvert.DeserializeObject<IList<EventDto>>(response);
            }
            catch (Exception)
            {
                return new List<EventDto>
                {
                    new EventDto
                    {
                        Name = "Feil",
                        Description = "Klarte ikke å laste arrangementer"
                    }
                };
            }
        }

        public async Task<EventDto> Get(int eventId)
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_Event, eventId));
                return JsonConvert.DeserializeObject<EventDto>(response);
            }
            catch (Exception)
            {
                return new EventDto
                {
                    Name = "Feil",
                    Description = "Klarte ikke å laste arrangement"
                };
            }
        }
    }
}