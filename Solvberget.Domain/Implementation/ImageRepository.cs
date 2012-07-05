using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;
using System.Linq;

namespace Solvberget.Domain.Implementation
{
    public class ImageRepository : IImageRepository
    {

        private IRepository _alephRepository;



        public string GetDocumentImage(string id)
        {
            if (string.IsNullOrEmpty(id))
                return string.Empty;

            _alephRepository = new AlephRepository();
            
            var doc = _alephRepository.GetDocument(id);

            if (doc == null)
                return string.Empty;

            if (Equals(doc.DocType, typeof(Film).Name))
                return GetFilmImage(doc as Film);

            //if (Equals(doc.DocType, typeof(Book).Name))
            //GetFilm(doc);

            //throw new NotImplementedException();
            return string.Empty;
        }

        public string GetDocumentThumbnailImage(string id, string size)
        {
            var posterUrl = GetDocumentImage(id);
            if (string.IsNullOrEmpty(posterUrl))
                return string.Empty;

            
            posterUrl = posterUrl.Replace("640.jpg", size != null ? size+".jpg" : "150.jpg");

            return posterUrl;
        }

        private string GetFilmImage(Film film)
        {

            // First; try IMDB
            var imdbObject = GetImdbObjectFromTitle(film.Title);

            if (isValidImdbMatch(film, imdbObject))
                return imdbObject.Poster;

            // Then try second source
            // TODO: Get a second source....

            return string.Empty;

        }

        private bool isValidImdbMatch(Film film, ImdbObject imdbObject)
        {

            if (imdbObject == null)
                return false;

            if (string.IsNullOrEmpty(imdbObject.Poster) || imdbObject.Poster.Equals("N/A"))
                return false;

            foreach (var person in film.InvolvedPersons)
            {
                if ( string.IsNullOrEmpty(person.Name))
                    continue;
                
                var personNames = person.Name.Split(',');
                var personName = personNames[1] + " " + personNames[0];
                personName = personName.Trim();

                if (imdbObject.Director.Split(',').Any(director => director.Trim().Equals(personName)))
                    return true;

                if (imdbObject.Writer.Split(',').Any(writer => writer.Trim().Equals(personName)))
                    return true;

            }

            return false;
        }

        private ImdbObject GetImdbObjectFromTitle(string title)
        {
            var imdbObjectAsJson = RepositoryUtils.GetJsonFromStreamWithParam(Properties.Settings.Default.ImdbApiUrl, title);

            if (imdbObjectAsJson != null)
                return new JavaScriptSerializer().Deserialize<ImdbObject>(imdbObjectAsJson);

            return null;

        }



    }
}