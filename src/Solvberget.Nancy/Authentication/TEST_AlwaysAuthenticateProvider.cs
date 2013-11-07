using Nancy.Security;

namespace Solvberget.Nancy.Authentication
{
    public class TEST_AlwaysAuthenticateProvider : IAuthenticationProvider
    {
        public IUserIdentity Authenticate(string username, string password)
        {
            return new UserIdentity(username);
        }
    }
}