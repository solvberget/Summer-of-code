using System.Collections.Generic;
using System.Linq;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Documents;
using Solvberget.Domain.Users;

namespace Solvberget.Domain.Favorites
{
    public class FavoritesRepository : IFavoritesRepository
    {
        private IRepository _documents;

        private Dictionary<string, List<string>> _favorites = new Dictionary<string, List<string>>();

        public FavoritesRepository(IRepository documents)
        {
            _documents = documents;
        }

        public IEnumerable<Favorite> GetFavorites(UserInfo user)
        {
            var favoritesForUser = GetFavoritesForUser(user);

            return favoritesForUser.Select(docId => new Favorite{User = user, Document = _documents.GetDocument(docId, true)});
        }

        public void AddFavorite(Document document, UserInfo user)
        {
            GetFavoritesForUser(user).Add(document.DocumentNumber);
        }

        public void RemoveFavorite(Document document, UserInfo user)
        {
            GetFavoritesForUser(user).Remove(document.DocumentNumber);
        }

        public bool IsFavorite(Document document, UserInfo user)
        {
            return GetFavoritesForUser(user).Contains(document.DocumentNumber);
        }

        private List<string> GetFavoritesForUser(UserInfo user)
        {
            List<string> favoritesForUser;

            if (!_favorites.TryGetValue(user.Id, out favoritesForUser))
            {
                favoritesForUser = new List<string>();
                _favorites[user.Id] = favoritesForUser;
            }
            return favoritesForUser;
        }
    }
}