using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IEventService
    {
        Task<IList<EventDto>> GetList();
        Task<EventDto> Get(int eventId);
    }
}
