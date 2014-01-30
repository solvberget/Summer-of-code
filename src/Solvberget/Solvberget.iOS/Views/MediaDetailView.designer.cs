// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace Solvberget.iOS
{
	[Register ("MediaDetailView")]
	partial class MediaDetailView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel HeaderLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView HeaderView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView Image { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel RatingSourceLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView StarsContainer { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel SubtitleLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel TypeLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (HeaderLabel != null) {
				HeaderLabel.Dispose ();
				HeaderLabel = null;
			}

			if (HeaderView != null) {
				HeaderView.Dispose ();
				HeaderView = null;
			}

			if (Image != null) {
				Image.Dispose ();
				Image = null;
			}

			if (RatingSourceLabel != null) {
				RatingSourceLabel.Dispose ();
				RatingSourceLabel = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (StarsContainer != null) {
				StarsContainer.Dispose ();
				StarsContainer = null;
			}

			if (SubtitleLabel != null) {
				SubtitleLabel.Dispose ();
				SubtitleLabel = null;
			}

			if (TypeLabel != null) {
				TypeLabel.Dispose ();
				TypeLabel = null;
			}
		}
	}
}
