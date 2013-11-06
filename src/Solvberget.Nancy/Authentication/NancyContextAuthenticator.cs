using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using Nancy;
using Nancy.Security;

namespace Solvberget.Nancy.Authentication
{
    public class NancyContextAuthenticator
    {
        private readonly IAuthenticationProvider _provider;

        public NancyContextAuthenticator(IAuthenticationProvider provider)
        {
            _provider = provider;
        }

        public IUserIdentity Authenticate(NancyContext context)
        {
            string username, password;
            if (!TryGetCredentialsFromRequest(context.Request, out username, out password)) return null;

            var userIdentity = GetCachedIdentityFor(username, password);

            if (null == userIdentity)
            {
                userIdentity = _provider.Authenticate(username, password);
                CacheUserIdentity(userIdentity, password);
            }

            return userIdentity;
        }

        private static bool TryGetCredentialsFromRequest(Request request, out string username, out string password)
        {
            username = request.Form.username.HasValue ? request.Form.username : request.Query.username;
            password = request.Form.password.HasValue ? request.Form.username : request.Query.password;

            return !String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password);
        }

        private void CacheUserIdentity(IUserIdentity userIdentity, string password)
        {
            if (null == userIdentity) return;
            _cachedIdentities[GetCacheKeyFor(userIdentity.UserName, password)] = userIdentity;
        }

        readonly Dictionary<string, IUserIdentity> _cachedIdentities = new Dictionary<string, IUserIdentity>(); 

        private IUserIdentity GetCachedIdentityFor(string username, string password)
        {
            IUserIdentity identity;
            _cachedIdentities.TryGetValue(GetCacheKeyFor(username, password), out identity);
            return identity;
        }

        private static string GetCacheKeyFor(string username, string password)
        {
            return "UID:" + FormsAuthentication.HashPasswordForStoringInConfigFile(username + password, FormsAuthPasswordFormat.SHA1.ToString());
        }
    }
}