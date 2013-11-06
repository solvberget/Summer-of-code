using Nancy;
using Nancy.Security;
using Solvberget.Nancy.Authentication;

namespace Solvberget.Nancy.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule(NancyContextAuthenticator authenticator) : base("/login")
        {
            Post["/"] = _ =>
            {
                var userIdentity = authenticator.Authenticate(Context);

                return null == userIdentity ? 
                      Response.AsJson(new {message = "Feil brukernavn eller passord"}, HttpStatusCode.Unauthorized) 
                    : Response.AsJson(new {message = "Autentisering vellykket."});
            };
        }
         
    }
}