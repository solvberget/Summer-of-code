using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;
using System.Web.Script.Serialization;

namespace Solvberget.Domain.Implementation
{
    public class LinticketRepository : IEventRepository

    {        
        public List<Event> GetEvents()
        {
            var events = new List<Event>();
            var eventsAsJson = RepositoryUtils.GetJsonFromStream(Properties.Settings.Default.LinticketUrl);
            
            if (eventsAsJson != null)
            {
                eventsAsJson = eventsAsJson.Replace("\"navn\"", "Name");
                eventsAsJson = eventsAsJson.Replace("\"sted\"", "Location");
                eventsAsJson = eventsAsJson.Replace("\"stedid\"", "LocationId");
                eventsAsJson = eventsAsJson.Replace("\"arrid\"", "Id");
                eventsAsJson = eventsAsJson.Replace("\"dato\"", "DateAsString");
                eventsAsJson = eventsAsJson.Replace("\"link\"", "Link");
                eventsAsJson = eventsAsJson.Replace("\"starttid\"", "Start");
                eventsAsJson = eventsAsJson.Replace("\"slutttid\"", "Stop");
                eventsAsJson = eventsAsJson.Replace("\"bilde\"", "PictureUrl");
                eventsAsJson = eventsAsJson.Replace("\"thumb\"", "ThumbUrl");
                eventsAsJson = eventsAsJson.Replace("\"tekst\"", "Description");
                eventsAsJson = eventsAsJson.Replace("\"teaser\"", "Teaser");
                eventsAsJson = eventsAsJson.Replace("\"typeid\"", "TypeId");
                eventsAsJson = eventsAsJson.Replace("\"lengdegrad\"", "Longitude");
                eventsAsJson = eventsAsJson.Replace("\"breddegrad\"", "Latitude");
                eventsAsJson = eventsAsJson.Replace("\"typenavn\"", "TypeName");
                eventsAsJson = eventsAsJson.Replace("\"by\"", "City");
                eventsAsJson = eventsAsJson.Replace("\"postnummer\"", "PostalCode");
                eventsAsJson = eventsAsJson.Replace("\"gateadresse\"", "Address");
                eventsAsJson = eventsAsJson.Replace("\"priser\"", "TicketPrice");

                events = new JavaScriptSerializer().Deserialize<List<Event>>(eventsAsJson);

            }

            return events;
        }
    }
}
