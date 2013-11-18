using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using SlidingPanels.Lib;
using System.Collections.Generic;
using SlidingPanels.Lib.PanelContainers;
using Solvberget.Core.ViewModels;

namespace MvxSlidingPanels.Touch.Views
{
	public partial class FirstView : MvxViewController
	{
		public new HomeViewModel ViewModel
		{
			get
			{
				return base.ViewModel as HomeViewModel;
			}
		}

		static bool UserInterfaceIdiomIsPhone
		{
			get
			{
				return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
			}
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

		public FirstView ()
			: base (UserInterfaceIdiomIsPhone ? "HomeView_iPhone" : "HomeView_iPad", null)
		{
			NavigationItem.LeftBarButtonItem = CreateSliderButton("Images/SlideRight40.png", PanelType.LeftPanel);
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();

//			this.AddBindings(
//				new Dictionary<object, string>()
//				{
//					{this, "Title DisplayName"},
//					{CenterText, "Text CenterText"},
//					{NavigateText, "Text DoNextLabel"},
//					{NavigateButton, "TouchUpInside DoNextCommand"}
//				});

			//NavigationController.NavigationBar.TintColor = UIColor.Black;
		}



		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear(animated);

		}
	}
}

