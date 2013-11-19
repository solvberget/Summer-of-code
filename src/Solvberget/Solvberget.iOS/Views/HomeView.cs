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
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace Solvberget.iOS
{
	public partial class HomeView : MvxViewController
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

		public HomeView() : base ("HomeView", null)
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

			var source = new MvxStandardTableViewSource(MenuTableView, UITableViewCellStyle.Default, new NSString("TableViewCell"), "TitleText Title;", UITableViewCellAccessory.None);

			MenuTableView.Source = source;

			var set = this.CreateBindingSet<HomeView, HomeViewModel>();
			set.Bind(source).To(vm => vm.MenuItems);
			//set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.);
			set.Apply();

			MenuTableView.ReloadData();

			View.AddSubview(MenuTableView);
		}



		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear(animated);
		}
	}
}

