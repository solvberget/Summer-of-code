using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Solvberget.Domain.Documents;
using Solvberget.Domain.Users;

namespace Solvberget.Domain.Favorites
{
    public interface IFavoritesRepository
    {
        IEnumerable<Favorite> GetFavorites(UserInfo user);
        void AddFavorite(Document document, UserInfo user);
        void RemoveFavorite(Document document, UserInfo user);
        bool IsFavorite(Document document, UserInfo user);
    }
}