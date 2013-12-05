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
	[Register ("SimpleCell")]
	partial class SimpleCell
	{
		[Outlet]
		MonoTouch.UIKit.UIView Box { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView Image { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Label1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Label2 { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Box != null) {
				Box.Dispose ();
				Box = null;
			}

			if (Image != null) {
				Image.Dispose ();
				Image = null;
			}

			if (Label1 != null) {
				Label1.Dispose ();
				Label1 = null;
			}

			if (Label2 != null) {
				Label2.Dispose ();
				Label2 = null;
			}
		}
	}
}
