using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using System.Globalization;

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
			StyleView();

			LoadingOverlay.LoadingText = "Henter blogg...";

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

			var labelSize = DescriptionLabel.SizeThatFits(new SizeF(View.Frame.Width-2*padding, 0f));

			DescriptionLabel.Frame = new RectangleF(new PointF(padding,padding), labelSize);
			DescriptionContainer.BackgroundColor = Application.ThemeColors.Hero;
			DescriptionContainer.Frame = new RectangleF(0, 0, View.Frame.Width, labelSize.Height + padding+padding);

			var y = padding;


			foreach (var s in ItemsContainer.Subviews)
				s.RemoveFromSuperview();

			foreach (var item in ViewModel.Posts)
			{
				var itemCtrl = new TitleAndSummaryItem();
				itemCtrl.Frame = new RectangleF(padding, y, View.Frame.Width - 2 * padding, 10);

				itemCtrl.Clicked += (sender, e) => ViewModel.ShowDetailsCommand.Execute(item);

				itemCtrl.TitleLabelText = item.Title;

				itemCtrl.SummaryLabelText = item.Published.ToString("dddd d. MMMM", new CultureInfo("nb-no")).ToUpperInvariant();

				if(!String.IsNullOrEmpty(item.Content)) itemCtrl.SummaryLabelText += Environment.NewLine + Environment.NewLine + item.Content.Replace("&nbsp;", " ").Trim();

				ItemsContainer.Add(itemCtrl.View);

				y += itemCtrl.Frame.Height + padding;
			}

			ItemsContainer.Frame = new RectangleF(0, DescriptionLabel.Frame.Bottom + padding, View.Frame.Width, y);
			ScrollContainer.ContentSize = new SizeF(ScrollContainer.Bounds.Width, y);
		}

		private void OnViewInBrowser(object sender, EventArgs e)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(ViewModel.Url));
		}
    }
}

