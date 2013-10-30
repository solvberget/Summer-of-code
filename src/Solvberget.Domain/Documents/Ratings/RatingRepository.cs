using System;
using System.Globalization;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Integrations.Bokelskere;
using Solvberget.Domain.Integrations.Imdb;

namespace Solvberget.Domain.Documents.Ratings
{

    public class RatingRepository : IRatingRepository
    {
        private readonly BokelskereRepository _bokelskereRepository;
        private readonly IRepository _documentRepository;

        public RatingRepository(IRepository documentRepository)
        {
            _documentRepository = documentRepository;
            _bokelskereRepository = new BokelskereRepository(_documentRepository);
        }

        public DocumentRating GetDocumentRating(string id)
        {
            var doc = _documentRepository.GetDocument(id, true);
            if (doc == null) return null;

            if (Equals(doc.DocType, typeof(Film).Name))
            {
                string rating = GetExternalFilmImdbRating(doc as Film);
                rating = rating.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                
                return new DocumentRating { Source = "IMDB", MaxScore = 10, Score = Double.Parse(rating), SourceUrl = "http://www.imdb.com" };
            }

            if (Equals(doc.DocType, typeof(Book).Name))
            {
                string rating = GetExternalBookElskereRating(doc as Book);
                rating = rating.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

                return new DocumentRating { Source = "Bokelskere", MaxScore = 6, Score = Double.Parse(rating), SourceUrl = "http://www.bokelskere.no" };
            }

            return null;
        }

        private string GetExternalBookElskereRating(Book book)
        {
           var bokElskereBook = _bokelskereRepository.GetExternalBokelskereBook(book.DocumentNumber);

           return bokElskereBook == null ? string.Empty : bokElskereBook.gjennomsnittelig_terningkast;
        }



        private static string GetExternalFilmImdbRating(Film film)
        {

            // --------------------------- IMDB ---------------------------

            var searchQuery = ImdbRepository.GetFilmSearchQuery(film);

            var imdbObject = ImdbRepository.GetImdbObjectFromSeachQuery(searchQuery);

            if (ImdbRepository.IsFilmValidImdbMatch(film, imdbObject))
                return imdbObject.imdbRating;

            if (!string.IsNullOrEmpty(film.OriginalTitle))
                imdbObject = ImdbRepository.GetImdbObjectFromSeachQuery(film.OriginalTitle);

            if (ImdbRepository.IsFilmValidImdbMatch(film, imdbObject))
                return imdbObject.imdbRating;

            // --------------------------- END IMDB ------------------------

            // Here we can try other sources if available

            return string.Empty;

        }


    }
}
