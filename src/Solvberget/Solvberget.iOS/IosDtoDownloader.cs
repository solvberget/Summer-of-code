using Solvberget.Core.ViewModels;
using Solvberget.Core.Services;
using Solvberget.Core.DTOs;
using Cirrious.MvvmCross.Views;
using System.Collections.Generic;

namespace Solvberget.iOS
{
    using Cirrious.CrossCore;
    using Cirrious.MvvmCross.Touch.Platform;
    using Cirrious.MvvmCross.Touch.Views.Presenters;
    using Cirrious.MvvmCross.ViewModels;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

	public class IosDtoDownloader : DtoDownloader
	{
		IMvxViewDispatcher _viewDispatcher;

		public IosDtoDownloader(IMvxViewDispatcher viewDispatcher, IStringDownloader stringDownloader) : base(stringDownloader)
		{
			_viewDispatcher = viewDispatcher;
		}

		public override async System.Threading.Tasks.Task<ListResult<TDto>> DownloadList<TDto>(string url, string method = "GET", bool ignoreError = false)
		{
			try
			{
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
				var result = await base.DownloadList<TDto>(url, method);

				if (!result.Success && !ignoreError)
				{
					HandleError(result.Reply);
				}

				return result;
			}
			finally
			{
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			}

		}

		public override async System.Threading.Tasks.Task<TDto> Download<TDto>(string url, string method = "GET", bool ignoreError = false)
		{
			try
			{
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

				var result = await base.Download<TDto>(url, method);

				if (!result.Success && !ignoreError)
				{
					HandleError(result.Reply);
				}

				return result;
			}
			finally
			{
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			}
		}

		void HandleError(string message)
		{
			if (message == Replies.RequireLoginReply)
			{
				_viewDispatcher.ShowViewModel(new MvxViewModelRequest(
					typeof(LoginViewModel),
					new MvxBundle(new Dictionary<string,string>{{ "navigateBackOnLogin","true" }}),
					null,
					null));

				return;
			}

			UIAlertView alert = new UIAlertView(UIScreen.MainScreen.Bounds);
			alert.Title = "Beklager, det gikk ikke..";
			alert.Message = message;
			alert.AddButton("Ok");
			alert.Show();
		}
	}
}