using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;

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

        public ImageRepository(IRepository documentRepository, IEnvironmentPathProvider environment)
        {
            var pathToImageCache = environment.GetImageCachePath();

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

            if (string.IsNullOrEmpty(id))
                return string.Empty;

            var cacheUrl = _storageHelper.GetLocalImageFileCacheUrl(id, false);
            if (!string.IsNullOrEmpty(cacheUrl))
                return cacheUrl;

            var doc = _documentRepository.GetDocument(id, true);
            
            return GetDocumentImage(id, null, doc, false);
        }

        public string GetDocumentThumbnailImage(string id, string size)
        {

            var cacheUrl = _storageHelper.GetLocalImageFileCacheUrl(size != null ? id + "-" + size : id, true);
            if (!string.IsNullOrEmpty(cacheUrl))
                return cacheUrl;

            var doc = _documentRepository.GetDocument(id, true);

            return GetDocumentImage(id, size, doc, true);
        }

        public string GetDocumentImage(string documentNumber, string size, Document doc, bool isThumbnail)
        {
            if (doc == null)
                return string.Empty;

            if (Equals(doc.DocType, typeof (Film).Name))
            {
                var posterUrl = GetExternalFilmImageUri(doc as Film);
                if (isThumbnail)
                    posterUrl = posterUrl.Replace("640.jpg", string.IsNullOrEmpty(size) ? size + ".jpg" : "60.jpg");

                return GetLocalImageUrl(posterUrl, size != null ? documentNumber + "-" + size : documentNumber, isThumbnail);
            }

            if (Equals(doc.DocType, typeof (Book).Name))
                return GetLocalImageUrl(GetExternalBookImageUri(doc as Book, isThumbnail), documentNumber, isThumbnail);

            if (Equals(doc.DocType, typeof (AudioBook).Name))
                return GetLocalImageUrl(GetExternalAudioBookImageUri(doc as AudioBook, isThumbnail),
                                        documentNumber, isThumbnail);

            if (Equals(doc.DocType, typeof (Cd).Name))
                return GetLocalImageUrl(GetExternalCdImageUri(doc as Cd, isThumbnail),  documentNumber, isThumbnail);
            return string.Empty;
        }


        private string GetExternalBookImageUri ( Book book, bool fetchThumbnail )
        {

            var isbn = book.Isbn;
            var xmlBook = new BokbasenRepository(_documentRepository).GenerateBookFromXml(_xmluri + "&ISBN=" + isbn);

            if (xmlBook == null) return string.Empty;

            if ( fetchThumbnail )
                return !string.IsNullOrEmpty(xmlBook.Thumb_Cover_Picture) ? xmlBook.Thumb_Cover_Picture : string.Empty;

            return !string.IsNullOrEmpty(xmlBook.Large_Cover_Picture) ? xmlBook.Large_Cover_Picture : string.Empty;
        }


        private string GetExternalAudioBookImageUri(AudioBook abook, bool fetchThumbnail)
        {

            var isbn = abook.Isbn;
            var xmlBook = new BokbasenRepository(_documentRepository).GenerateBookFromXml(_xmluri + "&ISBN=" + isbn);

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

            ImageCacheUtils.DownloadImageFromUrl(externalImageUrl, imageName, _pathToImageCache);

            var localServerUrl = Properties.Settings.Default.ServerUrl;
            var localImageCacheFolder = Properties.Settings.Default.ImageCacheFolder;
            return localServerUrl + localImageCacheFolder + imageName;
        }
    }
}
