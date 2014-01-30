using Cirrious.MvvmCross.Plugins.File;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class UserAuthenticationDataService : IUserAuthenticationDataService
    {
        private readonly IMvxFileStore _fileStore;

        public UserAuthenticationDataService(IMvxFileStore fileStore)
        {
            _fileStore = fileStore;
        }

        public bool UserInfoRegistered()
        {
            string output;
            
            var read = _fileStore.TryReadTextFile(GetPathForUserId(), out output);

            return read;
        }

        public string GetUserId()
        {
            string userId;

            var read = _fileStore.TryReadTextFile(GetPathForUserId(), out userId);

            return read ? userId : "Fant ikke brukerid";
        }

        public string GetUserPassword()
        {
            string pin;

            var read = _fileStore.TryReadTextFile(GetPathForPin(), out pin);

            return read ? pin : "Fant ikke pin";
        }

        public void SetUser(string userId)
        {
            _fileStore.WriteFile(GetPathForUserId(), userId);
        }

        public void SetPassword(string password)
        {
            _fileStore.WriteFile(GetPathForPin(), password);
        }

        public void RemoveUser()
        {
            _fileStore.DeleteFile(GetPathForUserId());
        }

        public void RemovePassword()
        {
            _fileStore.DeleteFile(GetPathForPin());
        }

        private string GetPathForUserId()
        {
            const string fileName = "UserId" + ".txt";

            _fileStore.EnsureFolderExists("Solvberget");

            var path = _fileStore.PathCombine("Solvberget", fileName);

            return path;
        }

        private string GetPathForPin()
        {
            const string fileName = "UserPin" + ".txt";

            _fileStore.EnsureFolderExists("Solvberget");

            var path = _fileStore.PathCombine("Solvberget", fileName);

            return path;
        }
    }
}