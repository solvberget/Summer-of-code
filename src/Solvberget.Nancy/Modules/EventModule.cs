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
                    Name = "Testarrangement", Description = "Testbeskrivelse",
                    Start = new DateTime(2013,11,30,15,00,00), End = new DateTime(2013,11,30, 16,00,00)
                },
                new EventDto
                {
                    Id = 2,
                    Name = "Julaften", Description = "Gaver til alle de snille barna",
                    Start = new DateTime(2013,12,24), End = new DateTime(2013,12,24), AllDay=true
                }
            };

            // todo: implement after new events integration in place
            Get["/"] = _ => testEvents;

            Get["/{id}"] = args => testEvents.FirstOrDefault(ev => ev.Id == args.id);
        }
    }
}