using System;
using System.Linq;
using Nancy;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Events;
using Solvberget.Nancy.Mapping;

namespace Solvberget.Nancy.Modules
{
    public class EventModule : NancyModule
    {
        public EventModule(IEventRepository eventRepository) : base("/events")
        {
            var testEvents = new[]
            {
                new EventDto
                {
                    Id = 1,
                    Name = "Åpningsfest", Description = "<p>11. januar 2014 åpner vi dørene. Da står et splitter nytt, lyst, åpent og innbydende bibliotektilbud klart til å ta imot regionens befolkning. Den nye og moderne biblioteksavdelingen i Sølvbergets første etasje blir Stavangers nye møteplass, og en slik nyhet må selvsagt markeres." +
                                                        "For å gjøre våre kjære bibliotekvenner kjent med de nye tilbudene og de nye arenaene inviterer vi til en fest som strekker seg fra tidlig morgen til sen kveld. Her er det noe for enhver smak.",
                    Location = "SF Kino", ImageUrl= "https://tuploads.s3.amazonaws.com/production/uploads/event/image/853/default_11jan_595x265.png",
                    TicketUrl = "https://solvberget.ticketco.no/apningsfest2",
                    Start = new DateTime(2014,1,11, 10,0,0), End = new DateTime(2014,1,11, 22,0,0),
                    TicketPrice = 0
                }
            };

            // todo: implement after new events integration in place
            Get["/"] = _ => testEvents.OrderByDescending(ev => ev.Start).ToArray();

            Get["/{id}"] = args => testEvents.FirstOrDefault(ev => ev.Id == args.id);
        }
    }
}