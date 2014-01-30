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
using System.Collections.Generic;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.iOS
{
    public class MvxSlidingPanelsTouchViewPresenter : MvxTouchViewPresenter
    {
        private UIWindow _window;
		private LeftPanelContainer _leftPanel;
		private Dictionary<Type, bool> _stackClearingViewModels;

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
			_stackClearingViewModels = new Dictionary<Type, bool>();

			RegisterStackClearingViewModel(typeof(HomeScreenViewModel));
			RegisterStackClearingViewModel(typeof(NewsListingViewModel));
			RegisterStackClearingViewModel(typeof(OpeningHoursViewModel));
			RegisterStackClearingViewModel(typeof(MyPageViewModel));
			RegisterStackClearingViewModel(typeof(SearchViewModel));
			RegisterStackClearingViewModel(typeof(SuggestionsListListViewModel));
			RegisterStackClearingViewModel(typeof(BlogOverviewViewModel));
			RegisterStackClearingViewModel(typeof(ContactInfoViewModel));
			RegisterStackClearingViewModel(typeof(EventListViewModel));
        }

        public override void ChangePresentation (Cirrious.MvvmCross.ViewModels.MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);
        }

		public override void Show(MvxViewModelRequest request) 
		{
			try 
			{
				var view = CurrentView = this.CreateViewControllerFor(request);

				// We clear the back stack up to now and hide the back button if this view should not allow viewstate popping
				if (_stackClearingViewModels.ContainsKey(request.ViewModelType)) {
					ClearBackStack();
				}

				if (_leftPanel != null)
				{
					SlidingPanelsController.HidePanel(_leftPanel);
				}

				//(SlidingPanelsController.NavigationItem.TitleView as UILabel).Text = "Sølvberget";


				Show(view);
			} catch (Exception e) 
			{
			}
		}

		public static IMvxTouchView CurrentView { get; private set;}

		public void ClearBackStack()
		{
			if (MasterNavigationController == null)
				return;
			MasterNavigationController.PopToViewController(_mainView, false);

		}

		UIViewController _mainView;
	
        protected override void ShowFirstView (UIViewController viewController)
        {
			_mainView = viewController;

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
					_leftPanel = new LeftPanelContainer(viewToAdd);
					_leftPanel.View.BackgroundColor = Application.ThemeColors.Main2;


					SlidingPanelsController.InsertPanel(_leftPanel);
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

			if (navController.RespondsToSelector(new MonoTouch.ObjCRuntime.Selector("interactivePopGestureRecognizer")))
			{
				navController.InteractivePopGestureRecognizer.Enabled = false;
			}

			navController.NavigationBar.SetTitleTextAttributes(new UITextAttributes()
				{
					TextColor = UIColor.White,
					TextShadowColor = UIColor.Clear,
					Font = Application.ThemeColors.HeaderFont
				});

			if (UIHelpers.MinVersion7)
			{
				navController.NavigationBar.BarTintColor = Application.ThemeColors.Main;
				navController.NavigationBar.TintColor = Application.ThemeColors.MainInverse;
				navController.NavigationBar.Translucent = false;
			}
			else
			{
				navController.NavigationBar.TintColor = Application.ThemeColors.Main;
			}

            return navController;
        }

        protected override void SetWindowRootViewController(UIViewController controller)
        {
            _window.AddSubview(RootController.View);
            _window.RootViewController = RootController;
        }


		public void RegisterStackClearingViewModel(Type viewModelType)
		{
			_stackClearingViewModels[viewModelType] = true;
		}
    }
}

