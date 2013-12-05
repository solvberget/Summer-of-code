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
	[Register ("SearchView")]
	partial class SearchView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel NoResultsLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISearchBar Query { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView Results { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Query != null) {
				Query.Dispose ();
				Query = null;
			}

			if (NoResultsLabel != null) {
				NoResultsLabel.Dispose ();
				NoResultsLabel = null;
			}

			if (Results != null) {
				Results.Dispose ();
				Results = null;
			}
		}
	}
}
