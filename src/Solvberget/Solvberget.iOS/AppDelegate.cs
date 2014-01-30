using Solvberget.Core.ViewModels;
using Solvberget.Core.Services;
using Solvberget.Core.DTOs;
using Cirrious.MvvmCross.Views;
using System.Collections.Generic;
using MonoTouch.FacebookConnect;

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
		const string FacebookAppId = "178416682352295";
		const string DisplayName = "Sølvberget for iOS";

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

			FBSettings.DefaultAppID = FacebookAppId;
			FBSettings.DefaultDisplayName = DisplayName;


            return true;
        }

		public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			// We need to handle URLs by passing them to FBSession in order for SSO authentication
			// to work.
			return FBSession.ActiveSession.HandleOpenURL(url);
		}

		public override void OnActivated (UIApplication application)
		{
			// We need to properly handle activation of the application with regards to SSO
			// (e.g., returning from iOS 6.0 authorization dialog or from fast app switching).
			FBSession.ActiveSession.HandleDidBecomeActive();
		}

    }

}