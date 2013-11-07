using System.Linq;
using Nancy;
using Nancy.Security;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Favorites;
using Solvberget.Nancy.Authentication;
using Solvberget.Nancy.Mapping;

namespace Solvberget.Nancy.Modules
{
    public class FavoritesModule : NancyModule
    {
        public FavoritesModule(IFavoritesRepository favorites, IRepository documents)
            : base("/favorites")
        {
            this.RequiresAuthentication();

            Get["/"] = _ => favorites.GetFavorites(Context.GetUserInfo()).Select(MapToDto);

            Delete["/{documentId}"] = args =>
            {
                var document = documents.GetDocument(args.documentId, true);
                favorites.RemoveFavorite(document, Context.GetUserInfo());

                return new { Success = true };
            };

            Put["/{documentId}"] = args =>
            {
                var document = documents.GetDocument(args.documentId, true);
                favorites.AddFavorite(document, Context.GetUserInfo());

                return new { Success = true };
            };
        }

        private object MapToDto(Favorite favorite)
        {
            return new FavoriteDto
            {
                Document = DtoMaps.Map(favorite.Document)
            };
        }
    }
}