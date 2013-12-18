using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Properties;

namespace Solvberget.Core.Services
{
    public class EventService : IEventService
    {
        private readonly DtoDownloader _downloader;

        public EventService(DtoDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<IList<EventDto>> GetList()
        {
            var response = await _downloader.DownloadList<EventDto>(Resources.ServiceUrl + Resources.ServiceUrl_Events);
            return response.Results;

        }

        public async Task<EventDto> Get(int eventId)
        {
            return await _downloader.Download<EventDto>(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_Event, eventId));
       }
    }
}