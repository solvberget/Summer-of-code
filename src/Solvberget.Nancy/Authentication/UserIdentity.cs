using System.Collections.Generic;
using Nancy.Security;

namespace Solvberget.Nancy.Authentication
{
    public class UserIdentity : IUserIdentity
    {
        public UserIdentity(string username)
        {
            UserName = username;
        }

        public string UserName { get; private set; }
        public IEnumerable<string> Claims { get; private set; }
    }
}