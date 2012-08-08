using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
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

        public string GetDocumentRating(string id)
        {
            var doc = _documentRepository.GetDocument(id, true);
            if (doc == null)
                return string.Empty;

            if (Equals(doc.DocType, typeof(Film).Name))
            {
                string rating = GetExternalFilmImdbRating(doc as Film);
                return rating;
            }

            if (Equals(doc.DocType, typeof(Book).Name))
            {
                string rating = GetExternalBookElskereRating(doc as Book);
                return rating;
            }

            return string.Empty;
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
