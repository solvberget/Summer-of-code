namespace Solvberget.Core.Services
{
    public interface IUserAuthenticationDataService
    {
        bool UserInfoRegistered();
        string GetUserId();
        string GetUserPassword();
        void SetUser(string userId);
        void SetPassword(string password);
    }

    public class UserAuthenticationDataService : IUserAuthenticationDataService
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