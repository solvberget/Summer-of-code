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
	[Register ("BlogPostSummaryItem")]
	partial class TitleAndSummaryItem
	{
		[Outlet]
		MonoTouch.UIKit.UILabel SummaryLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (SummaryLabel != null) {
				SummaryLabel.Dispose ();
				SummaryLabel = null;
			}
		}
	}
}
