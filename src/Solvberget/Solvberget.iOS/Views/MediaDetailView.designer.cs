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
		MonoTouch.UIKit.UIView HeaderView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView Image { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel MediaSubtitle { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel MediaTitle { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (HeaderView != null) {
				HeaderView.Dispose ();
				HeaderView = null;
			}

			if (Image != null) {
				Image.Dispose ();
				Image = null;
			}

			if (MediaTitle != null) {
				MediaTitle.Dispose ();
				MediaTitle = null;
			}

			if (MediaSubtitle != null) {
				MediaSubtitle.Dispose ();
				MediaSubtitle = null;
			}
		}
	}
}
