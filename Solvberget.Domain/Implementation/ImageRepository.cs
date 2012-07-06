using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
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
        private string _pathToImageCache;


        public ImageRepository(string pathToImageCache = null)
        {

            _pathToImageCache = string.IsNullOrEmpty(pathToImageCache)
    ? @"App_Data\"+Properties.Settings.Default.ImageCacheFolder : pathToImageCache;

        }

        private Document GetDocumentFromId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            _alephRepository = new AlephRepository();


            return _alephRepository.GetDocument(id);

        }

        public string GetDocumentImage(string id)
        {

            var doc = GetDocumentFromId(id);
            if (doc == null)
                return string.Empty;

            if (Equals(doc.DocType, typeof(Film).Name))
                return GetLocalImageUrl(GetFilmImage(doc as Film));

            //if (Equals(doc.DocType, typeof(Book).Name))
            //GetFilm(doc);

            //throw new NotImplementedException();
            return string.Empty;
        }

        public string GetDocumentThumbnailImage(string id, string size)
        {

            var doc = GetDocumentFromId(id);
            if (doc == null)
                return string.Empty;

            var posterUrl = string.Empty;
            if (Equals(doc.DocType, typeof(Film).Name))
                posterUrl = GetFilmImage(doc as Film);
            
            posterUrl = posterUrl.Replace("640.jpg", size != null ? size+".jpg" : "150.jpg");

            return GetLocalImageUrl(posterUrl);
        }

        private string GetFilmImage(Film film)
        {

            // --------------------------- IMDB ---------------------------

            var searchTitle = GetSearchTitle(film);

            var imdbObject = GetImdbObjectFromTitle(searchTitle);

            if (IsValidImdbMatch(film, imdbObject))
                return imdbObject.Poster;

            if(!string.IsNullOrEmpty(film.OriginalTitle))
                imdbObject = GetImdbObjectFromTitle(film.OriginalTitle);
         
            if (IsValidImdbMatch(film, imdbObject))
                return imdbObject.Poster;

            // --------------------------- END IMDB ------------------------

            // Here we can try other sources if available

            return string.Empty;

        }

        private static string GetSearchTitle(Film film)
        {
            var searchTitle = film.SeriesTitle + " " + film.SeriesNumber + " " + film.Title + " " + film.SubTitle;
            searchTitle = searchTitle.Replace("null", "");
            searchTitle = searchTitle.Trim();
            return searchTitle;
        }

        private string GetLocalImageUrl ( string externalImageUrl )
        {

            if (string.IsNullOrEmpty(externalImageUrl))
                return string.Empty;

            var imageName = Path.GetFileName(externalImageUrl);
            RepositoryUtils.DownloadImageFromUrl(externalImageUrl, imageName, _pathToImageCache);

            var serverUrl = Properties.Settings.Default.ServerUrl;
            var imageCacheFolder = Properties.Settings.Default.ImageCacheFolder;
            var internalmageUrl = serverUrl + imageCacheFolder + imageName;

            return internalmageUrl;
        }

        private static bool IsValidImdbMatch(Film film, ImdbObject imdbObject)
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

            return imdbObjectAsJson != null ? new JavaScriptSerializer().Deserialize<ImdbObject>(imdbObjectAsJson) : null;
        }



    }
}