using System;
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

        public IUserIdentity Authenticate(NancyContext context)
        {
            string username, password;
            if (!TryGetCredentialsFromRequest(context.Request, out username, out password)) return null;

            var userIdentity = _cache.GetCachedIdentityFor(username, password);

            if (null == userIdentity)
            {
                userIdentity = _provider.Authenticate(username, password);
                _cache.CacheUserIdentity(userIdentity, password);
            }

            return userIdentity;
        }

        private static bool TryGetCredentialsFromRequest(Request request, out string username, out string password)
        {
            username = request.Form.username.HasValue ? request.Form.username : request.Query.username;
            password = request.Form.password.HasValue ? request.Form.username : request.Query.password;

            return !String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password);
        }

    }
}