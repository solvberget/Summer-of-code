using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

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

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
		}

		LoadingOverlay loadingIndicator;

		UIScrollView container;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.Frame = new RectangleF(0,64, View.Frame.Width, View.Frame.Height - 64);
			View.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			container = new UIScrollView(new RectangleF(PointF.Empty, View.Frame.Size));

			View.Add(container);

			StyleView();

			ViewModel.WaitForReady(() => InvokeOnMainThread(RenderView));

			loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);

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

				itemCtrl.Clicked += (sender, e) => UIApplication.SharedApplication.OpenUrl(new NSUrl(item.Uri.OriginalString));

				itemCtrl.TitleLabelText = item.NewsTitle;

				if(!String.IsNullOrEmpty(item.Ingress)) itemCtrl.SummaryLabelText = item.Ingress.Replace("&nbsp;", " ");

				container.Add(itemCtrl.View);

				itemCtrl.Frame = new RectangleF(padding, y, itemCtrl.Frame.Width, itemCtrl.Frame.Height + padding);

				y += itemCtrl.Frame.Height + padding;
			}

			container.ContentSize = new SizeF(320, y);
			container.ScrollEnabled = true;

			loadingIndicator.Hide();
		}
    }
}

