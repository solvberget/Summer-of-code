using Nancy;
using Solvberget.Domain.Events;

namespace Solvberget.Nancy.Modules
{
    public class EventModule : NancyModule
    {
        public EventModule(IEventRepository eventRepository) : base("/events")
        {
            Get["/"] = _ => eventRepository.GetEvents();
        }
    }
}