using Nancy;
using Nancy.Security;
using Solvberget.Domain.Aleph;
using Solvberget.Nancy.Authentication;

namespace Solvberget.Nancy.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule(NancyContextAuthenticator authenticator, IRepository repository) : base("/login")
        {
            Post["/"] = _ =>
            {
                var userIdentity = authenticator.Authenticate(Request.Form.Username, Request.Form.Password) as AlephUserIdentity;
                
                return null == userIdentity ? 
                      Response.AsJson(new {message = "Feil brukernavn eller passord"}, HttpStatusCode.Unauthorized) 
                    : Response.AsJson(new {message = "Autentisering vellykket.", name = userIdentity.UserInfo.Name});
            };

            Get["/forgot/{userId}"] = args => repository.RequestPinCodeToSms(args.userId);
        }
    }
}