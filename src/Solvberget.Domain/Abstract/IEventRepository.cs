using System.Collections.Generic;

using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IEventRepository
    {
        List<Event> GetEvents();
    }
}