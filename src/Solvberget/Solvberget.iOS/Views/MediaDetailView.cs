using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public partial class MediaDetailView : NamedViewController
    {
		public new MediaDetailViewModel ViewModel
		{
			get
			{
				return base.ViewModel as MediaDetailViewModel;
			}
		}


        public MediaDetailView() : base("MediaDetailView", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
		}

		LoadingOverlay _loadingOverlay = new LoadingOverlay();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			RatingSourceLabel.Text = HeaderLabel.Text = SubtitleLabel.Text = TypeLabel.Text = String.Empty;


			Style();

			Add(_loadingOverlay);

			ViewModel.WaitForReady(() => InvokeOnMainThread(Update));

        }

		private void Style()
		{
			HeaderView.BackgroundColor = Application.ThemeColors.Main2;

			HeaderLabel.Font = Application.ThemeColors.TitleFont1;
			HeaderLabel.TextColor = Application.ThemeColors.MainInverse;

			SubtitleLabel.Font = Application.ThemeColors.DefaultFont;
			SubtitleLabel.TextColor = Application.ThemeColors.MainInverse;

			TypeLabel.Font = Application.ThemeColors.DefaultFont;
			TypeLabel.TextColor = Application.ThemeColors.MainInverse;
		}

		private void Update()
		{
			HeaderLabel.Text = ViewModel.Title;
			SubtitleLabel.Text = ViewModel.SubTitle;
			TypeLabel.Text = ViewModel.Type;
			RatingSourceLabel.Text = "Fra " + ViewModel.Rating.Source;

			var x = 0;

			for (int i = 0; i < (int)ViewModel.Rating.MaxScore; i++)
			{
				var star = new UIImageView(new RectangleF(x,0,12,12));

				if (i < ViewModel.Rating.Score)
				{
					star.Image = UIImage.FromBundle("/Images/star.on.png");
				}
				else
				{
					star.Image = UIImage.FromBundle("/Images/star.off.png");
				}

				StarsContainer.Add(star);
				x += 14;
			}

			Position();

			Image.Image = UIHelpers.ImageFromUrl(ViewModel.Image);

			_loadingOverlay.Hide();
		}

		float padding = 10.0f;

		private void Position()
		{
			var headerSize = HeaderLabel.SizeThatFits(new SizeF(HeaderLabel.Frame.Width, 0));

			HeaderLabel.Frame = new RectangleF(HeaderLabel.Frame.Location, headerSize);

			var subtitleSize = SubtitleLabel.SizeThatFits(new SizeF(SubtitleLabel.Frame.Width, 0));
			var subtitlePos = new PointF(SubtitleLabel.Frame.X, HeaderLabel.Frame.Bottom+padding);

			SubtitleLabel.Frame = new RectangleF(subtitlePos, subtitleSize);

			var typeSize = TypeLabel.SizeThatFits(new SizeF(TypeLabel.Frame.Width, 0));
			var typePos = new PointF(TypeLabel.Frame.X, SubtitleLabel.Frame.Bottom+padding);

			TypeLabel.Frame = new RectangleF(typePos, typeSize);
		}
    }
}

