using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Solvberget.Core.DTOs;

namespace Solvberget.iOS
{
	public class MyPageReservationsView : NamedTableViewController
	{
		public new MyPageReservationsViewModel ViewModel
		{
			get
			{
				return base.ViewModel as MyPageReservationsViewModel;
			}
		}


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var source = new SimpleTableViewSource<ReservationViewModel>(TableView, CellBindings.Reservations);
			TableView.Source = source;

			var loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);

			var set = this.CreateBindingSet<MyPageReservationsView, MyPageReservationsViewModel>();
			set.Bind(source).To(vm => vm.Reservations);
			//set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.RemoveReservation);

			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			ViewModel.WaitForReady(() => InvokeOnMainThread(OnViewModelReady));

			TableView.ReloadData();
		}

		private void OnViewModelReady()
		{
			if (ViewModel.Reservations.Count == 0)
			{
				Add(new UILabel(new RectangleF(10, 10, 300, 30)){ Text = "Du har ingen reservasjoner.", Font = Application.ThemeColors.DefaultFont });
			}
		}

	}
	
}
