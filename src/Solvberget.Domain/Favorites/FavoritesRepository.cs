using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Documents;
using Solvberget.Domain.Users;
using Solvberget.Domain.Utils;

namespace Solvberget.Domain.Favorites
{
    public class FavoritesRepository : IFavoritesRepository
    {
        private readonly IRepository _documents;
        private readonly IEnvironmentPathProvider _pathProvider;
        
        public FavoritesRepository(IRepository documents, IEnvironmentPathProvider pathProvider)
        {
            _documents = documents;
            _pathProvider = pathProvider;
        }

        public IEnumerable<Favorite> GetFavorites(UserInfo user)
        {
            var favoritesForUser = GetFavoritesForUser(user);

            return favoritesForUser.Select(docId => new Favorite{User = user, Document = _documents.GetDocument(docId, true)});
        }

        public void AddFavorite(Document document, UserInfo user)
        {
            var favs = GetFavoritesForUser(user);
            favs.Add(document.DocumentNumber);

            SaveFavorites(favs, user);
        }

        public void RemoveFavorite(Document document, UserInfo user)
        {
            var favs = GetFavoritesForUser(user);
            favs.Remove(document.DocumentNumber);

            SaveFavorites(favs, user);
        }

        private void SaveFavorites(List<string> favs, UserInfo user)
        {
            var file = _pathProvider.GetFavoritesPath(user.Id);
            File.WriteAllText(file, String.Join(";", favs));
        }

        public bool IsFavorite(Document document, UserInfo user)
        {
            return GetFavoritesForUser(user).Contains(document.DocumentNumber);
        }

        private List<string> GetFavoritesForUser(UserInfo user)
        {
            var file = _pathProvider.GetFavoritesPath(user.Id);

            if(!File.Exists(file)) return new List<string>();

            List<string> favoritesForUser = File.ReadAllText(file).Split(';').ToList();
            return favoritesForUser;
        }
    }
}