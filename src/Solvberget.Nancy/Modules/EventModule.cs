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
                    Name = "It's a wonderful life", Description = "Visning av \"It's a wonderful life\" har for lengst blitt en fast juletradisjon på Sølvberget. "
    + "Ingen kunne som Frank Capra fortelle historien om det hederlige, vanlige mennesket, og aldri gjorde han det vel bedre enn i It’s a Wonderful Life. Da filmen hadde premiere i julen 1946, syntes publikum den var deprimerende, og den ble ingen suksess. Nå regnes den som den kanskje største juleklassikeren av dem alle: En optistisk hyllest til livet, vennskapet og kjærligheten. "
    + "Visninger: Torsdag 12. desember klokken 18.00"
    + "Tirsdag 17. desember klokken 12.00. Sted: Sal 7, SF Kino.",
                    Location = "Musikk- og filmbiblioteket", ImageUrl= "https://tuploads.s3.amazonaws.com/production/uploads/event/image/816/default_Cinemateket-Den-ultimate-julefilmen_mainarticlethumbnail.png",
                    TicketUrl = "https://solvberget.ticketco.no/obokvartett",
                    Start = new DateTime(2013,12,17, 12,0,0), End = new DateTime(2013,12,17, 13,30,0),
                    TicketPrice = 0
                },

                new EventDto
                {
                    Id = 2,
                    Name = "Denne eventen har en lengre tittel",
                    Description = "Bla bla...",
                    Start = new DateTime(2013,12,17, 12,0,0), End = new DateTime(2013,12,17, 13,30,0),
                    Location = "SF Kino"
                },

                new EventDto
                {
                    Id = 3,
                    Name = "Denne eventen har en enda litt lengre tittel",
                    Description = "Bla bla...",
                    Start = new DateTime(2013,12,17, 12,0,0), End = new DateTime(2013,12,17, 13,30,0),
                    Location = "SF Kino"
                }
            };

            // todo: implement after new events integration in place
            Get["/"] = _ => testEvents.OrderByDescending(ev => ev.Start).ToArray();

            Get["/{id}"] = args => testEvents.FirstOrDefault(ev => ev.Id == args.id);
        }
    }
}