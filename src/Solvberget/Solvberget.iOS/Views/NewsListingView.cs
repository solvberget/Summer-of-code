using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public partial class NewsListingView : MvxViewController
    {
		public new NewsListingViewModel ViewModel
		{
			get
			{
				return base.ViewModel as NewsListingViewModel;
			}
		}

        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public NewsListingView()
			: base(UserInterfaceIdiomIsPhone ? "NewsListingView_iPhone" : "NewsListingView_iPad", null)
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
			
            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}

