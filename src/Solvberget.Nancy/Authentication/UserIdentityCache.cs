using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Security;
using Nancy.Security;

namespace Solvberget.Nancy.Authentication
{
    public class UserIdentityCache
    {
        public void CacheUserIdentity(IUserIdentity userIdentity, string password)
        {
            if (null == userIdentity) return;
            _cachedIdentities[GetCacheKeyFor(userIdentity.UserName, password)] = userIdentity;
        }

        readonly Dictionary<string, IUserIdentity> _cachedIdentities = new Dictionary<string, IUserIdentity>();

        public IUserIdentity GetCachedIdentityFor(string username, string password)
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