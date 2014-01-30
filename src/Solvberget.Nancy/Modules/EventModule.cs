using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Nancy;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Events;
using Solvberget.Domain.Utils;
using Solvberget.Nancy.Mapping;

namespace Solvberget.Nancy.Modules
{
    public class EventModule : NancyModule
    {
        public EventModule(IEnvironmentPathProvider env) : base("/events")
        {
            var events = new List<EventDto>();

            using (var file = new StreamReader(env.GetEventsPath(), Encoding.GetEncoding("ISO-8859-1")))
            {
                XDocument doc = XDocument.Load(file);

                foreach (var element in doc.XPathSelectElements("/events/event"))
                {
                    var ev = new EventDto();
                    events.Add(ev);

                    ev.Id = Int64.Parse(element.Descendants("id").First().Value);
                    ev.Name = element.Descendants("name").First().Value;
                    ev.Description = element.Descendants("description").First().Value;
                    ev.ImageUrl = element.Descendants("imageUrl").First().Value;
                    ev.Location = element.Descendants("location").First().Value;
                    ev.Start = DateTime.Parse(element.Descendants("start").First().Value);
                    ev.End = DateTime.Parse(element.Descendants("end").First().Value);
                    ev.TicketPrice = Double.Parse(element.Descendants("ticketPrice").First().Value);
                    ev.TicketUrl = element.Descendants("ticketUrl").First().Value;
                }
            }

            // todo: implement after new events integration in place
            Get["/"] = _ => events.OrderBy(ev => ev.Start).ToArray();

            Get["/{id}"] = args => events.FirstOrDefault(ev => ev.Id == args.id);
        }
    }
}