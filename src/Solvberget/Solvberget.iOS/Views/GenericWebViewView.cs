using System;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Solvberget.iOS
{
	public class GenericWebViewView : NamedViewController
    {
		private UIWebView _webView;

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

			_webView = new UIWebView(webFrame) {
				BackgroundColor = UIColor.White,
				ScalesPageToFit = true,
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
			};

			_webView.LoadStarted += delegate {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			};
			_webView.LoadFinished += delegate {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			};
			_webView.LoadError += (webview, args) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					_webView.LoadHtmlString (String.Format ("<html><center><font size=+5 color='red'>An error occurred:<br>{0}</font></center></html>", args.Error.LocalizedDescription), null);
			};

			View.AddSubview(_webView);

			_webView.LoadRequest(new NSUrlRequest(new NSUrl(ViewModel.Uri)));
		}

		public override void ViewWillDisappear (bool animated)
		{
			if (_webView != null)
			{
				_webView.StopLoading ();
				_webView.Delegate = null;
			}
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
		}
    }
}

