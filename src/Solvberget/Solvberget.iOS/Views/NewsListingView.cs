using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Globalization;

namespace Solvberget.iOS
{
	public class NewsListingView : NamedViewController
    {
		public new NewsListingViewModel ViewModel
		{
			get
			{
				return base.ViewModel as NewsListingViewModel;
			}
		}

		public NewsListingView() : base()
		{}

		UIScrollView container;

		public override void ViewDidLoad()
		{
			View.BackgroundColor = UIColor.White;
			base.ViewDidLoad();
		}

		protected override void ViewModelReady()
		{
			base.ViewModelReady();

			LoadingOverlay.LoadingText = "Henter nyheter...";

			if (null != container)
				container.RemoveFromSuperview();
		
			//View.Frame = new RectangleF(View.Frame);
			View.AutoresizingMask = UIViewAutoresizing.All;
			container = new UIScrollView(new RectangleF(PointF.Empty, View.Frame.Size));

			View.Add(container);

			StyleView();
			RenderView();
		}

		void StyleView()
		{
			container.BackgroundColor = UIColor.White;
		}

		private void RenderView()
		{

			var padding = 10.0f;

			var y = padding;

			foreach (var item in ViewModel.Stories)
			{
				var itemCtrl = new TitleAndSummaryItem();
				itemCtrl.View.Frame = new RectangleF(padding, y, container.Frame.Width - (2*padding), 50.0f);

				itemCtrl.Clicked += (sender, e) => UIApplication.SharedApplication.OpenUrl(new NSUrl(item.Uri.OriginalString));

				itemCtrl.TitleLabelText = item.NewsTitle;

				itemCtrl.SummaryLabelText = item.Published.ToString("dddd d. MMMM", new CultureInfo("nb-no")).ToUpperInvariant();

				if(!String.IsNullOrEmpty(item.Ingress)) itemCtrl.SummaryLabelText += Environment.NewLine + Environment.NewLine + item.Ingress.Replace("&nbsp;", " ");

				container.Add(itemCtrl.View);

				y += itemCtrl.Frame.Height + padding;
			}

			container.ContentSize = new SizeF(320, y);
			container.ScrollEnabled = true;
		}
    }
}

