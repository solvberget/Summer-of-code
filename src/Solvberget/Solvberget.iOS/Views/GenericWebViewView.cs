using System;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public class GenericWebViewView : MvxViewController
    {
		public new GenericWebViewViewModel ViewModel
		{
			get
			{
				return base.ViewModel as GenericWebViewViewModel;
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			var webFrame = UIScreen.MainScreen.ApplicationFrame;

			var webView = new UIWebView(webFrame) {
				BackgroundColor = UIColor.White,
				ScalesPageToFit = true,
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
			};


		
			webView.LoadStarted += delegate {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			};
			webView.LoadFinished += delegate {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			};
			webView.LoadError += (webview, args) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					webView.LoadHtmlString (String.Format ("<html><center><font size=+5 color='red'>An error occurred:<br>{0}</font></center></html>", args.Error.LocalizedDescription), null);
			};

			View.AddSubview(webView);

			webView.LoadRequest(new NSUrlRequest(new NSUrl(ViewModel.Uri)));


		}
    }
}

