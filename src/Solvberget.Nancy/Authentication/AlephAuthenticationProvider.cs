using Nancy.Security;
using Solvberget.Domain.Aleph;

namespace Solvberget.Nancy.Authentication
{
    public class AlephAuthenticationProvider : IAuthenticationProvider
    {
        private readonly IRepository _aleph;

        public AlephAuthenticationProvider(IRepository aleph)
        {
            _aleph = aleph;
        }

        public IUserIdentity Authenticate(string username, string password)
        {
            var userInfo = _aleph.GetUserInformation(username, password);

            if (null == userInfo) return null;

            return new AlephUserIdentity(username, password, userInfo);
        }
    }
}