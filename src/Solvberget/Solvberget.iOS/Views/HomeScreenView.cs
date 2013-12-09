using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{


	public partial class HomeScreenView : NamedViewController
    {
        public HomeScreenView() : base("HomeScreenView", null)
		{
			NavigationItem.LeftBarButtonItem = CreateSliderButton("/Images/logo.white.png", PanelType.LeftPanel);
        }

		public new HomeScreenViewModel ViewModel { get { return base.ViewModel as HomeScreenViewModel; }}

		private UIBarButtonItem CreateSliderButton(string imageName, PanelType panelType)
		{
			var button = new UIBarButtonItem(UIImage.FromBundle(imageName).Scale(new SizeF(30,30)), UIBarButtonItemStyle.Plain, 

				(s,e) => {
				SlidingPanelsNavigationViewController navController = NavigationController as SlidingPanelsNavigationViewController;
				navController.TogglePanel(panelType);
				});

			return button;
		}

        public override void ViewDidLoad()
		{
            base.ViewDidLoad();

			UILabel label = new UILabel(new RectangleF(20,20,280,100));
			label.Lines = 2;
			label.LineBreakMode = UILineBreakMode.WordWrap;
			label.Text = "Forside under utvikling. Dra menyen ut fra venstre kant.";

			ScrollView.Add(label);

        }
    }
}

