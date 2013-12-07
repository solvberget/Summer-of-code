using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Solvberget.Core.DTOs;

namespace Solvberget.iOS
{
	public class MyPageMessagesView : NamedTableViewController
	{
		public new MyPageMessagesViewModel ViewModel
		{
			get
			{
				return base.ViewModel as MyPageMessagesViewModel;
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var source = new SimpleTableViewSource<NotificationDto>(TableView, CellBindings.Messages);
			TableView.Source = source;

			var loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);

			var set = this.CreateBindingSet<MyPageMessagesView, MyPageMessagesViewModel>();
			set.Bind(source).To(vm => vm.Notifications);

			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			ViewModel.WaitForReady(() => InvokeOnMainThread(OnViewModelReady));

			TableView.ReloadData();
		}

		private void OnViewModelReady()
		{
			if (ViewModel.Notifications.Count == 0)
			{
				Add(new UILabel(new RectangleF(10, 10, 300, 30)){ Text = "Du har ingen meldinger.", Font = Application.ThemeColors.DefaultFont });
			}
		}
	}
	
}
