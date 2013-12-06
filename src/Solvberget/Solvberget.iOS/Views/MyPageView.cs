using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Solvberget.iOS
{
	public partial class MyPageView : NamedTabViewController
    {
        public MyPageView()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			var tab1 = new UIViewController();
			tab1.Title = "Lån + reservasjoner";

			var tab2 = new UIViewController();
			tab2.Title = "Favoritter";

			var tab3 = new UIViewController();
			tab3.Title = "Gebyrer";

			var tab4 = new UIViewController();
			tab4.Title = "Meldinger";

			var tab5 = new UIViewController();
			tab5.Title = "Profil";

			var tabs = new UIViewController []{
				tab1, tab2, tab3, tab4, tab5
			};

			ViewControllers = tabs;
			SelectedViewController = tab1;
        }
    }
}

