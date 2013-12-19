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
                    Name = "It's a wonderful life", Description = "<p>Visning av \"It's a wonderful life\" har for lengst blitt en fast juletradisjon på Sølvberget.</p> "
    + "<p>Ingen kunne som Frank Capra fortelle historien om det hederlige, vanlige mennesket, og aldri gjorde han det vel bedre enn i It’s a Wonderful Life. Da filmen hadde premiere i julen 1946, syntes publikum den var deprimerende, og den ble ingen suksess. Nå regnes den som den kanskje største juleklassikeren av dem alle: En optistisk hyllest til livet, vennskapet og kjærligheten.</p> "
    + "<p>Visninger: <br/>Torsdag 12. desember klokken 18.00"
    + "<br/>Tirsdag 17. desember klokken 12.00. <br/>Sted: Sal 7, SF Kino.",
                    Location = "SF Kino", ImageUrl= "https://tuploads.s3.amazonaws.com/production/uploads/event/image/816/default_Cinemateket-Den-ultimate-julefilmen_mainarticlethumbnail.png",
                    TicketUrl = "https://solvberget.ticketco.no/obokvartett",
                    Start = new DateTime(2013,12,17, 12,0,0), End = new DateTime(2013,12,17, 13,30,0),
                    TicketPrice = 0
                },

                new EventDto
                {
                    Id = 2,
                    ImageUrl="https://tuploads.s3.amazonaws.com/production/uploads/event/image/242/default_Bj_rnKallevig_Halvard.jpg",
                    Name = "Sanger fra 2. etasje: Dette har me venta på så lenge",
                    Description = "<p>Bjørn Kallevig og Halvard Josefsen leder oss gjennom kjente og kjære sanger som hører hjemme i adventstida. Ta deg en time fri fra julestria og bli med på viser som hører med til advent, vinter og jul. Bjørn Kallevig - gitar/sang og Halvard Josefsen - kontrabass.</p>" +
                                  "<p>Arrangementene er støttet av Den Kulturelle Spaserstokken.</p>",
                    Start = new DateTime(2013,12,16, 12,0,0), End = new DateTime(2013,12,16, 14,0,0),
                    Location = "Musikk- og filmbiblioteket"
                },

                new EventDto
                {
                    Id = 3,
                    ImageUrl="https://tuploads.s3.amazonaws.com/production/uploads/event/image/243/default_Stina_Kjelstad_02.jpg",
                    Name = "Biblåsessions: Stina Kjelstad",
                    Description = "<p>Kjelstad er født og oppvokst på Toten, og flyttet til Stavanger i 2009 for å studere sang og dans. I Stavanger har hun blant annet gjort seg bemerket med bandet Vindrosa, og i 2011 debuterte hun som soloartist med albumet Fading Slowly som består av egne låter på engelsk og norsk.</p><p> På Musikk- og filmbiblioteket spiller hun både nye og gamle låter.</p> " +
                                  "<p>Se hele programmet for høstens Biblåsessions.</p>",
                    Start = new DateTime(2013,12,17, 12,0,0), End = new DateTime(2013,12,17, 13,30,0),
                    Location = "Musikk- og filmbiblioteket"
                }
            };

            // todo: implement after new events integration in place
            Get["/"] = _ => testEvents.OrderByDescending(ev => ev.Start).ToArray();

            Get["/{id}"] = args => testEvents.FirstOrDefault(ev => ev.Id == args.id);
        }
    }
}