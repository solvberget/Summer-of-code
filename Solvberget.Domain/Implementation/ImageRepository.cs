using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;
using System.Linq;

namespace Solvberget.Domain.Implementation
{
    public class ImageRepository : IImageRepository
    {

        private readonly IRepository _documentRepository;
        private readonly string _pathToImageCache;

        private readonly string _serveruri = string.Empty;
        private readonly string[] _trueParams = { "SMALL_PICTURE", "LARGE_PICTURE", "PUBLISHER_TEXT", "FSREVIEW", "CONTENTS", "SOUND", "EXTRACT", "REVIEWS", "nocrypt" };
        private readonly string _serverSystem = string.Empty;
        private readonly string _xmluri = string.Empty;
        private readonly StorageHelper _storageHelper;

        public ImageRepository(IRepository documentRepository, string pathToImageCache = null)
        {


            _documentRepository = documentRepository;

            _pathToImageCache = string.IsNullOrEmpty(pathToImageCache)
    ? @"App_Data\"+Properties.Settings.Default.ImageCacheFolder : pathToImageCache;

            _storageHelper = new StorageHelper(_pathToImageCache);


            _serveruri = Properties.Settings.Default.BokBasenServerUri;
            _serverSystem = Properties.Settings.Default.BokBasenSystem;

            _xmluri = _serveruri;

            foreach (var param in _trueParams)
                _xmluri += param + "=true&";

            _xmluri += "SYSTEM=" + _serverSystem;
        }

        [OutputCache]
        public string GetDocumentImage(string id)
        {
            var cacheUrl = _storageHelper.GetLocalImageFileCacheUrl(id, false);
            if (!string.IsNullOrEmpty(cacheUrl))
                return cacheUrl;

            var doc = _documentRepository.GetDocument(id, false);
            if (doc == null)
                return string.Empty;

            if (Equals(doc.DocType, typeof(Film).Name))
                return GetLocalImageUrl(GetExternalFilmImageUri(doc as Film), id, false);

            if (Equals(doc.DocType, typeof(Book).Name))
                return GetLocalImageUrl(GetExternalBookImageUri(doc as Book, false), id, false);

            if (Equals(doc.DocType, typeof(AudioBook).Name))
                return GetLocalImageUrl(GetExternalAudioBookImageUri(doc as AudioBook, false), id, false);


            return string.Empty;
        }

        [OutputCache]
        public string GetDocumentThumbnailImage(string id, string size)
        {

            var cacheUrl = _storageHelper.GetLocalImageFileCacheUrl(size != null ? id + "-" + size : id, true);
            if (!string.IsNullOrEmpty(cacheUrl))
                return cacheUrl;

            var doc = _documentRepository.GetDocument(id, false);

            if (doc == null)
                return string.Empty;

            if (Equals(doc.DocType, typeof(Film).Name))
            {
                var posterUrl = GetExternalFilmImageUri(doc as Film);
                posterUrl = posterUrl.Replace("640.jpg", size != null ? size + ".jpg" : "60.jpg");
                return GetLocalImageUrl(posterUrl, size != null ? id + "-" + size : id, true);

            }
           
            if (Equals(doc.DocType, typeof(Book).Name))
                return GetLocalImageUrl(GetExternalBookImageUri(doc as Book, size == null || int.Parse(size) <= 60), id, true);
            
            if (Equals(doc.DocType, typeof(AudioBook).Name))
                return GetLocalImageUrl(GetExternalAudioBookImageUri(doc as AudioBook, size == null || int.Parse(size) <= 60), id, true);

            return string.Empty;
        }

       

        private string GetExternalBookImageUri ( Book book, bool fetchThumbnail )
        {

            var isbn = book.Isbn;
            var xmlBook = new BokBasenBook();

            xmlBook.FillProperties(_xmluri + "&ISBN="+isbn);

            if ( fetchThumbnail )
                return !string.IsNullOrEmpty(xmlBook.Thumb_Cover_Picture) ? xmlBook.Thumb_Cover_Picture : string.Empty;

            return !string.IsNullOrEmpty(xmlBook.Large_Cover_Picture) ? xmlBook.Large_Cover_Picture : string.Empty;
        }


        private string GetExternalAudioBookImageUri(AudioBook abook, bool fetchThumbnail)
        {

            var isbn = abook.Isbn;
            var xmlBook = new BokBasenBook();

            xmlBook.FillProperties(_xmluri + "&ISBN=" + isbn);

            if (fetchThumbnail)
                return !string.IsNullOrEmpty(xmlBook.Thumb_Cover_Picture) ? xmlBook.Thumb_Cover_Picture : string.Empty;

            return !string.IsNullOrEmpty(xmlBook.Large_Cover_Picture) ? xmlBook.Large_Cover_Picture : string.Empty;
        }


        private static string GetExternalFilmImageUri(Film film)
        {

            // --------------------------- IMDB ---------------------------

            var searchQuery = GetFilmSearchQuery(film);

            var imdbObject = GetImdbObjectFromSeachQuery(searchQuery);

            if (IsFilmValidImdbMatch(film, imdbObject))
                return imdbObject.Poster;

            if(!string.IsNullOrEmpty(film.OriginalTitle))
                imdbObject = GetImdbObjectFromSeachQuery(film.OriginalTitle);
         
            if (IsFilmValidImdbMatch(film, imdbObject))
                return imdbObject.Poster;

            // --------------------------- END IMDB ------------------------

            // Here we can try other sources if available

            return string.Empty;

        }


        private static string GetFilmSearchQuery(Film film)
        {
            var searchTitle = film.SeriesTitle + " " + film.SeriesNumber + " " + film.Title + " " + film.SubTitle;
            searchTitle = searchTitle.Replace("null", "").Trim();
            return searchTitle;
        }


        private static ImdbObject GetImdbObjectFromSeachQuery(string title)
        {
            var imdbObjectAsJson = RepositoryUtils.GetJsonFromStreamWithParam(Properties.Settings.Default.ImdbApiUrl, title);

            return imdbObjectAsJson != null ? new JavaScriptSerializer().Deserialize<ImdbObject>(imdbObjectAsJson) : null;
        }

        private static bool IsFilmValidImdbMatch(Film film, ImdbObject imdbObject)
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

        private string GetLocalImageUrl(string externalImageUrl, string id, bool isThumbnail)
        {

            if (string.IsNullOrEmpty(externalImageUrl))
                return string.Empty;

           var imageName = isThumbnail ? "thumb" + id + ".jpg" : id + ".jpg";

            RepositoryUtils.DownloadImageFromUrl(externalImageUrl, imageName, _pathToImageCache);

            var localServerUrl = Properties.Settings.Default.ServerUrl;
            var localImageCacheFolder = Properties.Settings.Default.ImageCacheFolder;
            return localServerUrl + localImageCacheFolder + imageName;

        }

    }
 
}
