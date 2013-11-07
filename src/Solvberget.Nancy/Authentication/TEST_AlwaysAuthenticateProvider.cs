using Nancy.Security;
using Solvberget.Domain.Users;

namespace Solvberget.Nancy.Authentication
{
    public class TEST_AlwaysAuthenticateProvider : IAuthenticationProvider
    {
        public IUserIdentity Authenticate(string username, string password)
        {
            return new AlephUserIdentity(username, new UserInfo{Id = username}); // test user
        }
    }
}