using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Solvberget.Core.ViewModels;
using System.Diagnostics;

namespace Solvberget.iOS
{
	public partial class SimpleCell : MvxTableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("SimpleCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("SimpleCell");

        public SimpleCell(IntPtr handle) : base(handle)
        {
        }

		bool _isFirstBinding = true;

		public override void Draw(RectangleF rect)
		{
			base.Draw(rect);

			if (_recalulatePositionsOnDraw)
			{
				PositionViews();
			}
		}

		bool _recalulatePositionsOnDraw;

		void PositionViews()
		{
			_recalulatePositionsOnDraw = false;

			Image.Hidden = null == Image.Image;

			var x = 10.0f;

			// show/hide image, center vertically
			if (!Image.Hidden)
			{
				x += Image.Frame.Width;

				var imageScale = Image.Frame.Width / Image.Image.Size.Width;
				var imageHeight = Math.Min(Image.Image.Size.Height * imageScale, Box.Frame.Height);
				var imageY = (Box.Frame.Height - imageHeight) / 2;

				var imageSize = new SizeF(Image.Frame.Width, imageHeight);
				Image.Frame = new RectangleF(new PointF(Image.Frame.X, imageY), imageSize);
			}

			// align text on x-axis
			Label1.Frame = new RectangleF(new PointF(x, Label1.Frame.Y), new SizeF(Box.Frame.Width - x, Label1.Frame.Height));
			Label2.Frame = new RectangleF(new PointF(x, Label2.Frame.Y), new SizeF(Box.Frame.Width - x, Label2.Frame.Height));

			// text height for width strategy
			var padding = String.IsNullOrEmpty(Label2.Text) ? 0.0f : 5.0f;
			var size1 = UIHelpers.CalculateHeightForWidthStrategy(this, Label1, Label1.Text);
			var size2 = String.IsNullOrEmpty(Label2.Text) ? SizeF.Empty : UIHelpers.CalculateHeightForWidthStrategy(this, Label2, Label2.Text);

			// align text on y-axis, centering vertically
			var totalHeight = size1.Height+size2.Height+padding;

			var y1 = (Box.Frame.Height - totalHeight) / 2;
			var y2 = y1 + size1.Height + padding;

			Label1.Frame = new RectangleF(new PointF(Label1.Frame.X, y1), size1);
			Label2.Frame = new RectangleF(new PointF(Label2.Frame.X, y2), size2);
		}

		public void Bind(string title, string subtitle, UIImage image)
		{
			_recalulatePositionsOnDraw = true;

			if (_isFirstBinding)
			{
				_isFirstBinding = false;
				SetThemeStyles();			
			}

			Label1.Text = title;
			Label2.Text = subtitle;
			Image.Image = image;
		}



		void SetThemeStyles()
		{
			Box.BackgroundColor = Application.ThemeColors.VerySubtle;

			Label1.Font = Application.ThemeColors.TitleFont;
			Label1.Lines = 2;
			Label1.LineBreakMode = UILineBreakMode.WordWrap;
			Label1.TextColor = Application.ThemeColors.Main;

			Label2.Font = Application.ThemeColors.DefaultFont;
			Label2.TextColor = Application.ThemeColors.Main2;
		}

        public static SimpleCell Create()
        {
            return (SimpleCell)Nib.Instantiate(null, null)[0];
        }
    }


}

