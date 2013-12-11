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
			DescriptionContainer.Frame = new RectangleF(0, 0, View.Frame.Width, DescriptionLabel.Frame.Height + padding*2);

			var y = padding;

			foreach (var s in ItemsContainer.Subviews)
				s.RemoveFromSuperview();

			foreach (var item in ViewModel.Posts)
			{
				var itemCtrl = new TitleAndSummaryItem();
				itemCtrl.View.Frame = new RectangleF(padding, y, ItemsContainer.Frame.Width - (2*padding), 50.0f);

				itemCtrl.Clicked += (sender, e) => UIApplication.SharedApplication.OpenUrl(new NSUrl(item.Url));

				itemCtrl.TitleLabelText = item.Title;

				if(!String.IsNullOrEmpty(item.Content)) itemCtrl.SummaryLabelText = item.Content.Replace("&nbsp;", " ");

				ItemsContainer.Add(itemCtrl.View);

				y += itemCtrl.Frame.Height + padding;
			}

			ScrollContainer.ContentSize = new SizeF(ScrollContainer.Bounds.Width, y);
		}

		private void OnViewInBrowser(object sender, EventArgs e)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(ViewModel.Url));
		}
    }
}

