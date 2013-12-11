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

    /// <summary>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the 
    /// User Interface of the application, as well as listening (and optionally responding) to 
    /// application events from iOS.
    /// </summary>
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        /// <summary>
        /// The window.
        /// </summary>
        private UIWindow window;

        /// <summary>
        /// Finished the launching.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="options">The options.</param>
        /// <returns>True or false.</returns>
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			this.window = new UIWindow(UIScreen.MainScreen.Bounds);

			UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;

            var presenter = new MvxSlidingPanelsTouchViewPresenter(this, this.window);
            var setup = new Setup(this, presenter);
            setup.Initialize();

			var appStart = new MvxAppStart<HomeScreenViewModel>();
			Mvx.RegisterSingleton<IMvxAppStart>(appStart);

			Mvx.LazyConstructAndRegisterSingleton<DtoDownloader, IosDtoDownloader>();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            this.window.MakeKeyAndVisible();

            return true;
        }
    }

	public class IosDtoDownloader : DtoDownloader
	{
		IMvxViewDispatcher _viewDispatcher;

		public IosDtoDownloader(IMvxViewDispatcher viewDispatcher, IStringDownloader stringDownloader) : base(stringDownloader)
		{
			_viewDispatcher = viewDispatcher;
		}

		public override async System.Threading.Tasks.Task<ListResult<TDto>> DownloadList<TDto>(string url, string method = "GET")
		{
			var result = await base.DownloadList<TDto>(url, method);

			if (!result.Success)
			{
				HandleError(result.Reply);
			}

			return result;
		}


		public override async System.Threading.Tasks.Task<TDto> Download<TDto>(string url, string method = "GET")
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
			alert.Title = "Uffda...";
			alert.Message = message;
			alert.AddButton("Ok");
			alert.Show();
		}
	}
}