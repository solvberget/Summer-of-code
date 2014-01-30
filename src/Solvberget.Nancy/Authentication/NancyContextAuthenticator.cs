using System;
using System.Linq;
using System.Text;
using System.Web;
using Nancy;
using Nancy.Security;

namespace Solvberget.Nancy.Authentication
{
    public class NancyContextAuthenticator
    {
        private readonly IAuthenticationProvider _provider;
        private UserIdentityCache _cache = new UserIdentityCache();

        public NancyContextAuthenticator(IAuthenticationProvider provider)
        {
            _provider = provider;
        }

        public IUserIdentity Authenticate(string username, string password)
        {
            var userIdentity = _cache.GetCachedIdentityFor(username, password);

            if (null == userIdentity)
            {
                userIdentity = _provider.Authenticate(username, password);
                _cache.CacheUserIdentity(userIdentity, password);
            }

            return userIdentity;
        }

        public IUserIdentity Authenticate(NancyContext context)
        {
            string username, password;
            if (!TryGetCredentialsFromRequest(context.Request, out username, out password)) return null;

            return Authenticate(username, password);
        }

        private static bool TryGetCredentialsFromRequest(Request request, out string username, out string password)
        {
            username = password = null;

            var header = request.Headers["Authorization"].ToList().SingleOrDefault();
            if (null == header) return false;

            var values = header.Split(':');
            if (values.Length != 2) return false;

            username = values[0];
            password = values[1];

            return !String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password);
        }
    }
}