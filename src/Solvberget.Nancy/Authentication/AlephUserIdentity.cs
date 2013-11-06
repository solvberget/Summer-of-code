using System.Collections.Generic;
using Nancy.Security;
using Solvberget.Domain.Users;

namespace Solvberget.Nancy.Authentication
{
    public class AlephUserIdentity : IUserIdentity
    {
        public AlephUserIdentity(string username, UserInfo userInfo)
        {
            UserName = username;
            UserInfo = userInfo;
        }

        public UserInfo UserInfo { get; private set; }

        public string UserName { get; private set; }
        public IEnumerable<string> Claims { get; private set; }
    }
}