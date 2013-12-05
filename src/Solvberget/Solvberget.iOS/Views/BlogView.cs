using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public partial class BlogView : NamedViewController
    {
        public BlogView() : base("BlogView", null)
		{
        }

		public new BlogViewModel ViewModel
		{
			get
			{
				return base.ViewModel as BlogViewModel;
			}
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		LoadingOverlay loadingIndicator;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			StyleView();

			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, OnViewInBrowser), true);

			ViewModel.WaitForReady(() => InvokeOnMainThread(RenderView));

			loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);

		}

		void StyleView()
		{
			DescriptionLabel.TextColor = Application.ThemeColors.Main;
			DescriptionLabel.Font = Application.ThemeColors.DefaultFont;
			DescriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
			DescriptionLabel.Lines = 0;
		}

		private void RenderView()
		{
			var padding = 10.0f;

			DescriptionLabel.Text = "The blog description should go here. Need to refactor IBlogService to populate a complete BlogViewModel with Url, Description and Posts, not just Posts.";
			DescriptionLabel.SizeToFit();

			DescriptionContainer.BackgroundColor = Application.ThemeColors.Hero;
			DescriptionContainer.Frame = new RectangleF(0, 0, 320, DescriptionLabel.Frame.Height + padding*2);

			var y = padding;

			foreach (var post in ViewModel.Posts)
			{
				var postView = new BlogPostSummaryItem();

				postView.TitleLabelText = post.Title;
				postView.SummaryLabelText = post.Description;

				ItemsContainer.Add(postView.View);
				postView.Frame = new RectangleF(padding, y, postView.Frame.Width, postView.Frame.Height + padding);

				y += postView.Frame.Height + padding;
			}

			var icY = DescriptionContainer.Frame.Y + DescriptionContainer.Frame.Height;

			ItemsContainer.Frame = new RectangleF(0, icY , 320, y);

			ScrollContainer.ContentSize = new SizeF(320, y + icY);
			ScrollContainer.ScrollEnabled = true;

			loadingIndicator.Hide();
		}

		private void OnViewInBrowser(object sender, EventArgs e)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(ViewModel.Url));
		}
    }
}

