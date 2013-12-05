using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;

namespace Solvberget.iOS
{
	public partial class HomeScreenView : MvxViewController
    {
        public HomeScreenView() : base("HomeScreenView", null)
		{
			NavigationItem.LeftBarButtonItem = CreateSliderButton("Images/SlideRight40.png", PanelType.LeftPanel);
			//NavigationItem.Title = "Sølvberget";


        }

		private UIBarButtonItem CreateSliderButton(string imageName, PanelType panelType)
		{
			UIButton button = new UIButton(new RectangleF(0, 0, 40f, 40f));
			button.SetBackgroundImage(UIImage.FromBundle(imageName), UIControlState.Normal);
			button.TouchUpInside += delegate
			{
				SlidingPanelsNavigationViewController navController = NavigationController as SlidingPanelsNavigationViewController;
				navController.TogglePanel(panelType);
			};

			return new UIBarButtonItem(button);
		}

		public override void ViewWillDisappear(bool animated)
		{
			if (null != NavigationItem)
			{
				NavigationItem.Title = String.Empty;
			}
		}

		public override void ViewWillAppear(bool animated)
		{
			NavigationItem.Title = "Sølvberget";
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

