using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    class LastFmAlbum
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string SmallImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
        
        public void FillProperties (XElement element)
        {

            var name = element.Element("name");
            if (name != null) Name = name.Value;

            var artist = element.Element("artist");
            if (artist != null) Artist = artist.Value;

            var smallimage = element.Elements("image").Where(x => ((string)x.Attribute("size")).Equals("medium")).Select(x => x.Value).FirstOrDefault();
            
            SmallImageUrl = smallimage;

            var largeimage = element.Elements("image").Where(x => ((string)x.Attribute("size")).Equals("extralarge")).Select(x => x.Value).FirstOrDefault();

            LargeImageUrl = largeimage;


        }
    }
}
