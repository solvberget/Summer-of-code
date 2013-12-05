using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Solvberget.iOS
{
    public partial class TitleAndSummaryItem : UIViewController
    {
		public TitleAndSummaryItem() : base("TitleAndSummaryItem", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

		public string TitleLabelText { get; set;
		}

		public string SummaryLabelText
		{
			get;
			set;
		}

		public RectangleF Frame
		{
			get { return View.Frame; }
			set { View.Frame = value; }
		}

		void StyleView()
		{
			TitleLabel.Font = Application.ThemeColors.TitleFont;
			TitleLabel.TextColor = Application.ThemeColors.Main;
			SummaryLabel.Font = Application.ThemeColors.DefaultFont;
			SummaryLabel.TextColor = Application.ThemeColors.Main2;
			View.BackgroundColor = Application.ThemeColors.VerySubtle;
		}

		void UpdateLayout()
		{
			TitleLabel.Text = TitleLabelText;
			SummaryLabel.Text = SummaryLabelText;
			TitleLabel.Frame = new RectangleF(TitleLabel.Frame.Location, TitleLabel.SizeThatFits(new SizeF(TitleLabel.Frame.Width, 0)));
			var padding = 15.0f;


			SummaryLabel.Frame = new RectangleF(new PointF(SummaryLabel.Frame.X, TitleLabel.Frame.Y + TitleLabel.Frame.Height + padding), 
				SummaryLabel.SizeThatFits(new SizeF(SummaryLabel.Frame.Width, 0)));




			var totalHeight = TitleLabel.Frame.Y + TitleLabel.Frame.Height +
			                  SummaryLabel.Frame.Height + padding;

			View.Frame = new RectangleF(View.Frame.Location, new SizeF(View.Frame.Width, totalHeight));

			//TitleLabel.BackgroundColor = UIColor.Red.ColorWithAlpha(0.5f);
			//SummaryLabel.BackgroundColor = UIColor.Blue.ColorWithAlpha(0.5f);


		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			StyleView();
			UpdateLayout();

			View.AddGestureRecognizer(new UITapGestureRecognizer(OnTap));
        }

		public event EventHandler Clicked = (s,e) => {};

		private void OnTap()
		{
			Clicked(this, EventArgs.Empty);
		}
    }
}

