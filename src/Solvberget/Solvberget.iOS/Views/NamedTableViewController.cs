using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.iOS
{
	public abstract class NamedTableViewController : MvxTableViewController
	{
		LoadingOverlay _loadingOverlay = new LoadingOverlay();

		public LoadingOverlay LoadingOverlay { get { return _loadingOverlay; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (null == ViewModel) return;
			NavigationItem.Title = (ViewModel as BaseViewModel).Title.ToUpperInvariant();

			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			_loadingOverlay.Show(View);
		}

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}

		public override void ViewWillDisappear(bool animated)
		{
			if (UIHelpers.MinVersion7)
			{
				NavigationItem.Title = String.Empty;
			}

			base.ViewWillDisappear(animated);

		}

		public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
		{
			if (ViewModel is BaseViewModel)
			{
				((BaseViewModel)ViewModel).WaitForReady(() => InvokeOnMainThread(ViewModelReady));
			}
		}

		protected virtual void ViewModelReady()
		{
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
			_loadingOverlay.Hide();
		}

		public override void ViewWillAppear(bool animated)
		{
			if (null != ViewModel)
			{
				var vm = (BaseViewModel)ViewModel;
				vm.OnViewReady();
				vm.WaitForReady(() => InvokeOnMainThread(ViewModelReady));
				NavigationItem.Title = vm.Title.ToUpperInvariant();
			}

			base.ViewWillAppear(animated);
		}
	}
	
}
