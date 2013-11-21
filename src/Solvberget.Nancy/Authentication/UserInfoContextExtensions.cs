using System.Threading;
using Autofac;
using Nancy;
using Nancy.Bootstrappers.Autofac;
using Solvberget.Domain.Aleph;
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

        /// <summary>
        /// Ensures the currently authenticated IUser gets a fresh UserInfo object from Aleph at some point in the (near) future
        /// </summary>
        /// <param name="context"></param>
        public static void RequireUserInfoRefresh(this NancyContext context)
        {
            var user = context.GetAlephUserIdentity();

            if (null == user) return;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                user.UserInfo = Bootstrapper.Container.Resolve<IRepository>().GetUserInformation(user.UserName, user.Password);
            });
        }
    }
}