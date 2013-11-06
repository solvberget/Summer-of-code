using Nancy.Security;

namespace Solvberget.Nancy.Authentication
{
    public interface IAuthenticationProvider
    {
        IUserIdentity Authenticate(string username, string password);
    }
}