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

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			LoadingOverlay.LoadingText = "Henter blogg...";

			StyleView();

			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, OnViewInBrowser), true);
		}

		void StyleView()
		{
			DescriptionLabel.TextColor = Application.ThemeColors.Main;
			DescriptionLabel.Font = Application.ThemeColors.DefaultFont;
			DescriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
			DescriptionLabel.Lines = 0;
		}

		protected override void ViewModelReady()
		{
			base.ViewModelReady();

			var padding = 10.0f;

			DescriptionLabel.Text = ViewModel.Description;
			DescriptionLabel.SizeToFit();

			DescriptionContainer.BackgroundColor = Application.ThemeColors.Hero;
			DescriptionContainer.Frame = new RectangleF(0, 0, 320, DescriptionLabel.Frame.Height + padding*2);

			var y = padding;

			foreach (var s in ItemsContainer.Subviews)
				s.RemoveFromSuperview();

			foreach (var post in ViewModel.Posts)
			{
				var postCtrl = new TitleAndSummaryItem();

				postCtrl.Clicked += (sender, e) => post.Show(ViewModel.Id);

				postCtrl.TitleLabelText = post.Title;
				postCtrl.SummaryLabelText = post.Description;

				ItemsContainer.Add(postCtrl.View);
				postCtrl.Frame = new RectangleF(padding, y, postCtrl.Frame.Width, postCtrl.Frame.Height + padding);

				y += postCtrl.Frame.Height + padding;
			}

			var icY = DescriptionContainer.Frame.Y + DescriptionContainer.Frame.Height;

			ItemsContainer.Frame = new RectangleF(0, icY , 320, y);


			ScrollContainer.ContentSize = new SizeF(320, y + icY);
			ScrollContainer.ScrollEnabled = true;
		}

		private void OnViewInBrowser(object sender, EventArgs e)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(ViewModel.Url));
		}
    }
}

