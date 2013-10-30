using System.Collections.Generic;

namespace Solvberget.Domain.Events
{
    public interface IEventRepository
    {
        List<Event> GetEvents();
    }
}