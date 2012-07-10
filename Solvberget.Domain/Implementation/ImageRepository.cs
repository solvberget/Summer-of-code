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

        private static readonly IRepository AlephRepository = new AlephRepository();
        private string _pathToImageCache;

        private readonly string _serveruri = string.Empty;
        private readonly string[] _trueParams = { "SMALL_PICTURE", "LARGE_PICTURE", "PUBLISHER_TEXT", "FSREVIEW", "CONTENTS", "SOUND", "EXTRACT", "REVIEWS", "nocrypt" };
        private readonly string _serverSystem = string.Empty;
        private readonly string _xmluri = "";


        public ImageRepository(string pathToImageCache = null)
        {

            _pathToImageCache = string.IsNullOrEmpty(pathToImageCache)
    ? @"App_Data\"+Properties.Settings.Default.ImageCacheFolder : pathToImageCache;

            _serveruri = Properties.Settings.Default.BokBasenServerUri;
            _serverSystem = Properties.Settings.Default.BokBasenSystem;

            _xmluri = _serveruri;
            foreach (var param in _trueParams)
            {
                _xmluri += param + "=true&";
            }
            _xmluri += "SYSTEM=" + _serverSystem;
        }


        public string GetDocumentImage(string id)
        {

            var doc = AlephRepository.GetDocument(id, false);
            if (doc == null)
                return string.Empty;

            if (Equals(doc.DocType, typeof(Film).Name))
                return GetLocalImageUrl(GetFilmImage(doc as Film), false);

            if (Equals(doc.DocType, typeof(Book).Name))
                return GetLocalImageUrl(GetBookImage(doc as Book, false), false);

            if (Equals(doc.DocType, typeof(AudioBook).Name))
                return GetLocalImageUrl(GetAudioBookImage(doc as AudioBook, false), false);



            //throw new NotImplementedException();
            return string.Empty;
        }

        public string GetDocumentThumbnailImage(string id, string size)
        {

            var doc = AlephRepository.GetDocument(id, false);

            if (doc == null)
                return string.Empty;

            var posterUrl = string.Empty;
            if (Equals(doc.DocType, typeof(Film).Name))
            {
                posterUrl = GetFilmImage(doc as Film);
                posterUrl = posterUrl.Replace("640.jpg", size != null ? size + ".jpg" : "150.jpg");
                return GetLocalImageUrl(posterUrl, true);

            }
            if (Equals(doc.DocType, typeof(Book).Name))
                return GetLocalImageUrl(GetBookImage(doc as Book, size == null || int.Parse(size) <= 150), true);
            if (Equals(doc.DocType, typeof(AudioBook).Name))
                return GetLocalImageUrl(GetAudioBookImage(doc as AudioBook, size == null || int.Parse(size) <= 150), true);


            return string.Empty;
        }

        private string GetBookImage ( Book book, bool fetchThumbnail )
        {

            var isbn = book.Isbn;
            var xmlBook = new BokBasenBook();
            xmlBook.FillProperties(_xmluri + "&ISBN="+isbn);
            if ( fetchThumbnail )
                return !string.IsNullOrEmpty(xmlBook.Thumb_Cover_Picture) ? xmlBook.Thumb_Cover_Picture : string.Empty;

            return !string.IsNullOrEmpty(xmlBook.Large_Cover_Picture) ? xmlBook.Large_Cover_Picture : string.Empty;
        }


        private string GetAudioBookImage(AudioBook abook, bool fetchThumbnail)
        {

            var isbn = abook.Isbn;
            var xmlBook = new BokBasenBook();
            xmlBook.FillProperties(_xmluri + "&ISBN=" + isbn);
            if (fetchThumbnail)
                return !string.IsNullOrEmpty(xmlBook.Thumb_Cover_Picture) ? xmlBook.Thumb_Cover_Picture : string.Empty;

            return !string.IsNullOrEmpty(xmlBook.Large_Cover_Picture) ? xmlBook.Large_Cover_Picture : string.Empty;
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

        private string GetLocalImageUrl ( string externalImageUrl, bool isThumbnail )
        {

            if (string.IsNullOrEmpty(externalImageUrl))
                return string.Empty;

            var imageName = Path.GetFileName(externalImageUrl);
            if (imageName != null) imageName = imageName.Replace("-", "");
            if (imageName != null) imageName = imageName.Replace("+", "");
            if (isThumbnail)
                imageName = "thumb" + imageName;

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
