using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels;
using Solvberget.Droid.Views;

namespace Solvberget.Droid.Helpers
{
    public class AndroidDtoDownloader : DtoDownloader
    {
        public AndroidDtoDownloader(IStringDownloader downloader) : base(downloader)
        {
        }

        public override async Task<ListResult<TDto>> DownloadList<TDto>(string url, string method = "GET")
        {
            var result = await base.DownloadList<TDto>(url, method);
            if (!result.Success)
            {
                HandleError(result.Reply);
            }

            return result;
        }

        public override async Task<TDto> Download<TDto>(string url, string method = "GET")
        {
            var result = await base.Download<TDto>(url, method);
            if (!result.Success)
            {
                HandleError(result.Reply);
            }

            return result;
        }

        void HandleError(string message)
        {
            var act = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            var ctx = act.ApplicationContext;
            if (message == Replies.RequireLoginReply)
            {
                var mvxAct = act as HomeView;
                if (mvxAct != null)
                {
                    mvxAct.Show(new MvxViewModelRequest(
                    typeof(LoginViewModel),
                    new MvxBundle(new Dictionary<string, string> { { "navigateBackOnLogin", "true" } }),
                    null,
                    null));
                }

                return;
            }
            
            Toast.MakeText(ctx, "Kunne desverre ikke laste data.", ToastLength.Long).Show();
        }
    }
}