using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.File.Droid;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services.Stubs
{
    public class UserAuthenticationTemporaryStub : IUserAuthenticationDataService
    {
        private readonly MvxFileStore _fileStore = new MvxAndroidFileStore();

        public bool UserInfoRegistered()
        {
            return true;
        }

        public string GetUserId()
        {
            //return "164916";

            string userId;

            var path = GetPathForUserId();

            var read = _fileStore.TryReadTextFile(path, out userId);

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
