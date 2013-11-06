using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Solvberget.Nancy.Authentication;

namespace Solvberget.Nancy.Modules
{
    public class FavoritesModule : NancyModule
    {
        public FavoritesModule()
            : base("/favorites")
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                return "todo..";
            };
        }
         
    }
}