using System.Collections.Generic;
using Nancy.Security;
using Solvberget.Domain.Users;

namespace Solvberget.Nancy.Authentication
{
    public class AlephUserIdentity : IUserIdentity
    {
        public AlephUserIdentity(string username, string password, UserInfo userInfo)
        {
            UserName = username;
            Password = password;
            UserInfo = userInfo;
        }

        public UserInfo UserInfo { get; private set; }

        public string UserName { get; private set; }
        public string Password { get; set; }
        public IEnumerable<string> Claims { get; private set; }
    }
}