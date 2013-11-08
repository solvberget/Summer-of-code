using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services.Stubs
{
    public class UserAuthenticationTemporaryStub : IUserAuthenticationDataService
    {
        public bool UserInfoRegistered()
        {
            return true;
        }

        public string GetUserId()
        {
            return "164916";
        }

        public string GetUserPassword()
        {
            return "9236";
        }

        public void SetUser(string userId)
        {
            throw new System.NotImplementedException();
        }

        public void SetPassword(string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
