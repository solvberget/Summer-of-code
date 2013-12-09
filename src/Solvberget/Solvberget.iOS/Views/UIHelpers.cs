using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Linq;

namespace Solvberget.iOS
{
	public static class UIHelpers
	{
		public static UIImage ImageFromUrl (string uri)
		{
			if (String.IsNullOrEmpty(uri)) return null;

			using (var url = new NSUrl (uri))
			using (var data = NSData.FromUrl(url))
				return null != data ? UIImage.LoadFromData(data) : null;
		}

		public static SizeF CalculateHeightForWidthStrategy(UIView context, UILabel label, string text)
		{
			var maxSize = new SizeF(label.Frame.Width, float.MaxValue);

			var size = context.StringSize(text, label.Font, maxSize, UILineBreakMode.WordWrap);
			return size;
		}

		public static void SetContentSize(UIScrollView scrollView)
		{
			if (scrollView.Subviews.Length == 0)
			{
				scrollView.ContentSize = SizeF.Empty;
				return;
			}

			scrollView.ContentSize = new SizeF(320, scrollView.Subviews.Last().Frame.Bottom + 10.0f);
		}
	}
}

