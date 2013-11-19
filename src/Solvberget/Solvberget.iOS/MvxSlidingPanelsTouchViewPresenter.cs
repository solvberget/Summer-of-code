// 
// As seen in SlidingPanels.Touch sample project 
//
using System;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using MonoTouch.UIKit;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Views;
using System.Drawing;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
    public class MvxSlidingPanelsTouchViewPresenter : MvxTouchViewPresenter
    {
        private UIWindow _window;

        public SlidingPanelsNavigationViewController SlidingPanelsController 
        {
            get
            {
                return MasterNavigationController as SlidingPanelsNavigationViewController;
            }
        }

        public UIViewController RootController 
        {
            get;
            private set;
        }

        public MvxSlidingPanelsTouchViewPresenter(UIApplicationDelegate applicationDelegate, UIWindow window) :
            base(applicationDelegate, window)
        {
            _window = window;
        }

        public override void ChangePresentation (Cirrious.MvvmCross.ViewModels.MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);
        }

		public override void Show(MvxViewModelRequest request) 
		{
			ClearBackStack();
			try 
			{
				var view = this.CreateViewControllerFor(request);
				Show(view);
			} catch (Exception) 
			{
			}
		}

		public void ClearBackStack()
		{
			if (MasterNavigationController == null)
				return;

			MasterNavigationController.PopToRootViewController (true);
			MasterNavigationController = null;
		}
	
        protected override void ShowFirstView (UIViewController viewController)
        {
            // Show the first view
            base.ShowFirstView (viewController);

            // create the Sliding Panels View Controller and make it a child controller
            // of the root controller
            RootController.AddChildViewController (SlidingPanelsController);
            RootController.View.AddSubview (SlidingPanelsController.View);

            // use the first view to create the sliding panels 
			AddPanel<HomeViewModel>(PanelType.LeftPanel, viewController as MvxViewController);
        }

        protected void AddPanel<T>(PanelType panelType, MvxViewController mvxController) where T : MvxViewModel
        {
            UIViewController viewToAdd = (UIViewController) mvxController.CreateViewControllerFor<T>();

            switch (panelType)
            {
                case PanelType.LeftPanel:
                    SlidingPanelsController.InsertPanel(new LeftPanelContainer(viewToAdd));
                    break;

                    case PanelType.RightPanel:
                    SlidingPanelsController.InsertPanel(new RightPanelContainer(viewToAdd));
                    break;

                    case PanelType.BottomPanel:
                    SlidingPanelsController.InsertPanel(new BottomPanelContainer(viewToAdd));
                    break;

                    default:
                    throw new ArgumentException("PanelType is invalid");
            };
        }

        protected override UINavigationController CreateNavigationController (UIViewController viewController)
        {
            SlidingPanelsNavigationViewController navController = new SlidingPanelsNavigationViewController (viewController);
            RootController = new UIViewController ();
            return navController;
        }

        protected override void SetWindowRootViewController(UIViewController controller)
        {
            _window.AddSubview(RootController.View);
            _window.RootViewController = RootController;
        }

    }
}

