using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;

namespace Solvberget.Domain.Implementation
{
    class ImdbRepository
    {

        public static string GetFilmSearchQuery(Film film)
        {
            var searchTitle = film.SeriesTitle + " " + film.SeriesNumber + " " + film.Title + " " + film.SubTitle;
            searchTitle = searchTitle.Replace("null", "").Trim();
            return searchTitle;
        }

        public static ImdbObject GetImdbObjectFromSeachQuery(string title)
        {
            var imdbObjectAsJson = RepositoryUtils.GetJsonFromStreamWithParam(Properties.Settings.Default.ImdbApiUrl, title);

            return imdbObjectAsJson != null ? new JavaScriptSerializer().Deserialize<ImdbObject>(imdbObjectAsJson) : null;
        }

        public static bool IsFilmValidImdbMatch(Film film, ImdbObject imdbObject)
        {

            if (imdbObject == null)
                return false;

            if (string.IsNullOrEmpty(imdbObject.Poster) || imdbObject.Poster.Equals("N/A"))
                return false;

            foreach (var person in film.InvolvedPersons)
            {
                if (string.IsNullOrEmpty(person.Name))
                    continue;

                var personNames = person.Name.Split(',');
                string personName;
                if (personNames.Length > 1)
                    personName = personNames[1] + " " + personNames[0];
                else
                    personName = personNames[0];

                personName = personName.Trim();

                if (imdbObject.Director.Split(',').Any(director => director.Trim().Equals(personName)))
                    return true;

                if (imdbObject.Writer.Split(',').Any(writer => writer.Trim().Equals(personName)))
                    return true;

            }

            return false;
        }


    }
}
