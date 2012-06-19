using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Film : Document
    {

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            
        }

        public static Film GetFilmFromFindDocXml(string xml)
        {
            var film = new Film();

            film.FillProperties(xml);

            return film;
        }
    }
}
