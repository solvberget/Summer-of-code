namespace Solvberget.Core.Services.Interfaces
{
    public interface IUserAuthenticationDataService
    {
        bool UserInfoRegistered();
        string GetUserId();
        string GetUserPassword();
        void SetUser(string userId);
        void SetPassword(string password);
        void RemoveUser();
        void RemovePassword();
    }
}