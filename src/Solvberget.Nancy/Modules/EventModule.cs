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
                    Name = "Biblåsessions: Strykekvartett fra SSO", Description = "Stavanger symfoniorkester stiller med en strykekvartett. De vil spille variert program fra mange sjangere. Fra Mozart til Beatles. Konserten er gratis.",
                    Location = "Musikk- og filmbiblioteket", ImageUrl= "https://tuploads.s3.amazonaws.com/production/uploads/event/image/761/thumb_Bue.jpg",
                    TicketUrl = "https://solvberget.ticketco.no/obokvartett",
                    Start = new DateTime(2013,11,2), End = new DateTime(2013,11,22)
                },
                new EventDto
                {
                    Id = 2,
                    Name = "Bokprat med baby", Description = "Bokprat med baby er et tilbud til alle som er hjemme med baby som ønsker gode tips til bøker om barseltid, barnets utvikling, den nye familiesituasjonen og gode bøker for de minste som skaper leselyst både for den lille og deg. Tilbudet har blitt veldig populært og tilbakemeldingene fra deltakerne er at dette er både lærerikt og sosialt.",
                    Location = "Barne- og ungdomsbiblioteket", ImageUrl= "https://tuploads.s3.amazonaws.com/production/uploads/event/image/253/thumb_bokpratmbaby.png",
                    TicketUrl = "https://solvberget.ticketco.no/bokpratmbabynov",
                    Start = new DateTime(2013,11,6,12,0,0), End = new DateTime(2013,11,6,13,0,0)
                },
                new EventDto
                {
                    Id = 3,
                    Name = "Kulturbiblioteket", Description = "Hver torsdag blir det som vanlig bokprat klokken 12.00 i Kulturbiblioteket.",
                    Location = "Barne- og ungdomsbiblioteket", ImageUrl= "https://tuploads.s3.amazonaws.com/production/uploads/event/image/262/thumb_Skjermbilde_2013-11-01_kl._12.15.22_PM.png",
                    TicketUrl = "https://solvberget.ticketco.no/bokpratkleppa",
                    Start = new DateTime(2013,11,7,12,0,0), End = new DateTime(2013,11,7,13,0,0)
                }
            };

            // todo: implement after new events integration in place
            Get["/"] = _ => testEvents.OrderByDescending(ev => ev.Start).ToArray();

            Get["/{id}"] = args => testEvents.FirstOrDefault(ev => ev.Id == args.id);
        }
    }
}