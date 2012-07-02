using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solvberget.Domain.DTO
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Link { get; set; }
        public string PictureUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string Description { get; set; }
        public string Teaser { get; set; }
        public string TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
