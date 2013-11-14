using Nancy;
using Solvberget.Domain.Users;

namespace Solvberget.Nancy.Authentication
{
    public static class UserInfoContextExtensions
    {
        public static UserInfo GetUserInfo(this NancyContext context)
        {
            var alephUserInfo = context.CurrentUser as AlephUserIdentity;
            if (null == alephUserInfo) return null;

            return alephUserInfo.UserInfo;
        }

        public static AlephUserIdentity GetAlephUserIdentity(this NancyContext context)
        {
            var alephUserInfo = context.CurrentUser as AlephUserIdentity;
            if (null == alephUserInfo) return null;

            return alephUserInfo;
        }
    }
}