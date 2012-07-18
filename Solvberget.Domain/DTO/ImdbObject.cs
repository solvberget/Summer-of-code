using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Solvberget.Domain.Utils;

namespace Solvberget.Domain.DTO
{
    class ImdbObject
    {

        public string Title { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public string Runtime { get; set; }
        public string imdbRating { get; set; }
        public string imdbVotes { get; set; }
        public string Genre { get; set; }
        public string Released { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }

    }
}
