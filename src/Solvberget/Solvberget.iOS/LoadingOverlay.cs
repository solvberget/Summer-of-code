//
// Lifted from Xamarin recipes documentation:
// http://docs.xamarin.com/recipes/ios/standard_controls/popovers/display_a_loading_message/
//

using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace Solvberget.iOS
{
	public class LoadingOverlay : UIView {

		UILabel loadingLabel;
		UIActivityIndicatorView activitySpinner;

		public void Show(UIView container)
		{
			RemoveFromSuperview();
			container.Add(this);

			// reset 
			foreach (var s in Subviews)
				s.RemoveFromSuperview();

			Frame = container.Bounds;

			// configurable bits
			BackgroundColor = UIColor.White;
			AutoresizingMask = UIViewAutoresizing.All;

			var centeredBox = new UIView(new RectangleF(0,0,Frame.Width, 100));
			centeredBox.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			centeredBox.Center = new PointF(Frame.Width / 2, Frame.Height / 2);

			// create the activity spinner, center it horizontall and put it 5 points above center x
			activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			activitySpinner.Color = Application.ThemeColors.Main;
			activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			activitySpinner.Center = new PointF(centeredBox.Frame.Width / 2, (centeredBox.Frame.Height / 2) - 60);

			centeredBox.Add(activitySpinner);
			activitySpinner.StartAnimating ();

			// create and configure the "Loading Data" label
			loadingLabel = new UILabel();
			loadingLabel.BackgroundColor = UIColor.Clear;
			loadingLabel.Font = Application.ThemeColors.MenuFont;
			loadingLabel.TextColor = Application.ThemeColors.Main2;
			loadingLabel.Text = LoadingText;
			loadingLabel.TextAlignment = UITextAlignment.Center;
			loadingLabel.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;

			loadingLabel.Frame = new RectangleF(0, 0, centeredBox.Frame.Width, 50);
			loadingLabel.Center = new PointF(centeredBox.Frame.Width / 2, centeredBox.Frame.Height / 2);

			centeredBox.Add(loadingLabel);
			Add(centeredBox);
		}

		private string _loadingText = "Laster...";

		public string LoadingText
		{
			set {

				_loadingText = value;

				if (null == loadingLabel) return;
				loadingLabel.Text = value;
			}
			get { return _loadingText; }
		}

		/// <summary>
		/// Fades out the control and then removes it from the super view
		/// </summary>
		public void Hide ()
		{
			UIView.Animate (
				0.5, // duration
				() => { Alpha = 0; },
				() => { 
					RemoveFromSuperview(); 
					Alpha = 1.0f;
				}
			);
		}
	};
}

