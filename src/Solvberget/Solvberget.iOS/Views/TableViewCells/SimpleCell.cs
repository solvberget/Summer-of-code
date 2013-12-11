using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Solvberget.Core.ViewModels;
using System.Diagnostics;

namespace Solvberget.iOS
{
	public partial class SimpleCell : MvxTableViewCell, ISimpleCell
    {
        public static readonly UINib Nib = UINib.FromName("SimpleCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("SimpleCell");

        public SimpleCell(IntPtr handle) : base(handle)
        {
			//ContentMode = UIViewContentMode.Redraw;
        }

		bool _isFirstBinding = true;

		public string ImageUrl {get;set;}

		public void SetImage(UIImage image)
		{
			// show/hide image, center vertically
			if (null != image)
			{
				var imageWidth = 60.0f;

				var imageScale = imageWidth / image.Size.Width;
				var imageHeight = Math.Min(image.Size.Height * imageScale, box.Frame.Height);
				var imageY = (box.Frame.Height - imageHeight) / 2;

				var imageSize = new SizeF(imageWidth, imageHeight);
				var imageView = new UIImageView(new RectangleF(new PointF(0, imageY), imageSize));
				imageView.Image = image;

				box.Add(imageView);
			}
		}

		UIView box;

		public float TableWidth {get;set;}

		public void Bind(string title, string subtitle)
		{
			if (null != box)
			{
				box.RemoveFromSuperview();
			}

			var padding = 10.0f;
			float widthMinusPadding = TableWidth - (2 * padding);

			if (_isFirstBinding)
			{
				_isFirstBinding = false;
				var bgBox = new UIView(new RectangleF(padding,padding,widthMinusPadding,80));
				SelectedBackgroundView = new UIView();
				SelectedBackgroundView.Add(bgBox);
				SelectedBackgroundView.BackgroundColor = UIColor.White;
				bgBox.BackgroundColor = Application.ThemeColors.Hero;
			}

			box = new UIView(new RectangleF(padding, padding, widthMinusPadding, 80));

			var x = 70.0f;

			// align text on x-axis
			var l1Frame = new RectangleF(new PointF(x, 0), new SizeF(box.Frame.Width - x, 0));

			var label1 = new UILabel(l1Frame);
			label1.BackgroundColor = UIColor.Clear;
			label1.Font = Application.ThemeColors.TitleFont;
			label1.Lines = 0;
			label1.LineBreakMode = UILineBreakMode.WordWrap;
			label1.TextColor = Application.ThemeColors.Main2;

			#if DEBUG
			//label1.BackgroundColor = UIColor.Red.ColorWithAlpha(0.5f);
			#endif

			label1.Text = title;

			UILabel label2 = null;

			var size1 = UIHelpers.CalculateHeightForWidthStrategy(this, label1, title);
			SizeF size2 = SizeF.Empty;

			if (!String.IsNullOrEmpty(subtitle))
			{
				var l2Frame = new RectangleF(new PointF(x, 0), new SizeF(box.Frame.Width - x, 0));
				label2 = new UILabel(l2Frame);
				label2.BackgroundColor = UIColor.Clear;
				label2.Font = Application.ThemeColors.SubTitleFont;
				label2.TextColor = Application.ThemeColors.SubTitleColor;
				label2.Text = subtitle;
				label2.Lines = 0;
				label2.LineBreakMode = UILineBreakMode.WordWrap;
				size2 = UIHelpers.CalculateHeightForWidthStrategy(this, label2, subtitle);

				#if DEBUG
				//label2.BackgroundColor = UIColor.Blue.ColorWithAlpha(0.5f);
				#endif
			}

			// text height for width strategy
			var textYPadding = String.IsNullOrEmpty(title) ? 0.0f : 0.0f;

			// align text on y-axis, centering vertically
			var totalHeight = size1.Height+size2.Height+textYPadding;

			var y1 = (box.Frame.Height - totalHeight) / 2;
			var y2 = y1 + size1.Height + textYPadding;

			label1.Frame = new RectangleF(new PointF(label1.Frame.X, y1), size1);

			if (null != label2)
			{
				label2.Frame = new RectangleF(new PointF(label2.Frame.X, y2), size2);

			}

			box.BackgroundColor = Application.ThemeColors.VerySubtle;

			box.Add(label1);
			if (null != label2) box.Add(label2);

			Add(box);
		}

        public static SimpleCell Create()
        {
            return (SimpleCell)Nib.Instantiate(null, null)[0];
        }
    }


}

