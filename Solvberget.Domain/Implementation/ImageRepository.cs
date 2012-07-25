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

            if (Equals(doc.DocType, typeof(Cd).Name))
                return GetLocalImageUrl(GetExternalCdImageUri(doc as Cd, false), id, false);


            return string.Empty;
        }

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

            if (Equals(doc.DocType, typeof(Cd).Name))
            {
                var test = GetLocalImageUrl(GetExternalCdImageUri(doc as Cd, true), size != null ? id + "-" + size : id, true);
                return test;
            }
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

            var searchQuery = ImdbRepository.GetFilmSearchQuery(film);

            var imdbObject = ImdbRepository.GetImdbObjectFromSeachQuery(searchQuery);

            if (ImdbRepository.IsFilmValidImdbMatch(film, imdbObject))
                return imdbObject.Poster;

            if(!string.IsNullOrEmpty(film.OriginalTitle))
                imdbObject = ImdbRepository.GetImdbObjectFromSeachQuery(film.OriginalTitle);

            if (ImdbRepository.IsFilmValidImdbMatch(film, imdbObject))
                return imdbObject.Poster;

            // --------------------------- END IMDB ------------------------

            // Here we can try other sources if available

            return string.Empty;

        }

        private static string GetExternalCdImageUri(Cd cd, bool isThumbnail)
        {
            // --------------------------- LAST.FM -------------------------
            var searchQuery = LastFmRepository.GetLastFmSearchQuery(cd);

            var lastFmAlbum = LastFmRepository.GetLastFmAlbumFromSeachQuery(searchQuery);

            if (lastFmAlbum != null){
                if (isThumbnail) return lastFmAlbum.SmallImageUrl;
                return lastFmAlbum.LargeImageUrl;
            }
            // --------------------------- END LAST.FM ---------------------
            
            // Here we can try other sources if available

            return string.Empty;
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
