using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using System.Globalization;

namespace Solvberget.iOS
{
	public partial class BlogPostView : NamedViewController
    {
        public BlogPostView() : base("BlogPostView", null)
        {
        }

		public new BlogPostViewModel ViewModel
		{
			get
			{
				return base.ViewModel as BlogPostViewModel;
			}
		}

        public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			LoadingOverlay.LoadingText = "Henter bloggpost...";

			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, OnViewInBrowser), true);
		}

		protected override void ViewModelReady()
		{
			var styles = "<style>body { font-family: Open Sans }</style>";
			var html = styles + ViewModel.Content + "<p style=color:gray>Publisert av " + ViewModel.Author + ", " + ViewModel.Published.ToString("ddd d. MMMM yyyy", new CultureInfo("nb-no"));

			WebView.LoadHtmlString(html, null);

			base.ViewModelReady();
		}

		private void OnViewInBrowser(object sender, EventArgs e)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(ViewModel.Url));
		}
    }
}

