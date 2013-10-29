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
    }
}
