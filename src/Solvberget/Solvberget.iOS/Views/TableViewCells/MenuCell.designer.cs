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
	[Register ("MenuCell")]
	partial class MenuCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel _iconLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel _titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_iconLabel != null) {
				_iconLabel.Dispose ();
				_iconLabel = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}
		}
	}
}
