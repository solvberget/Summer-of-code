using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;

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

		LoadingOverlay _loadingOverlay;

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, OnViewInBrowser), true);

			ViewModel.WaitForReady(() => InvokeOnMainThread(RenderView));

			_loadingOverlay = new LoadingOverlay();
			Add(_loadingOverlay);
        }

		void RenderView()
		{
			_loadingOverlay.Hide();

			/*
			TitleAndSummaryItem ctrl = new TitleAndSummaryItem();
			ctrl.TitleLabelText = "Skrevet av " + ViewModel.Author + ", " + ViewModel.Published.ToString("d. MMM");
			ctrl.SummaryLabelText = ViewModel.Content;
			ScrollContainer.Add(ctrl.View);
			ctrl.View.BackgroundColor = UIColor.Clear;
			ctrl.View.Frame = new RectangleF(new PointF(10, 0), ctrl.View.SizeThatFits(new SizeF(320, 0)));
			ScrollContainer.ContentSize = ctrl.View.Frame.Size;*/

			var styles = "<style>body { font-family: Open Sans }</style>";
			var html = styles + ViewModel.Content + "<p style=color:gray>Publisert av " + ViewModel.Author + ", " + ViewModel.Published.ToString("ddd d. MMM yyyy");


			WebView.LoadHtmlString(html, null);
		}

		private void OnViewInBrowser(object sender, EventArgs e)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(ViewModel.Url));
		}
    }
}

