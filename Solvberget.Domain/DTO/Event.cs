using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string Start { get; set; }
        public string Stop { get; set; }
        public string Link { get; set; }
        public string PictureUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string Description { get; set; }
        public string Teaser { get; set; }
        public string TypeId { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string TypeName { get; set; }
    }
}
