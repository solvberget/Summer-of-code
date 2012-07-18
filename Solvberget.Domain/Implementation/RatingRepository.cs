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

        private readonly IRepository _documentRepository;

        public RatingRepository(IRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public string GetDocumentRating(string id)
        {
            var doc = _documentRepository.GetDocument(id, false);
            if (doc == null)
                return string.Empty;

            if (Equals(doc.DocType, typeof(Film).Name))
            {
                string rating = GetExternalFilmImdbRating(doc as Film);
                return rating;
            }

            return string.Empty;
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
