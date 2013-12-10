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

		protected override void ViewModelReady()
		{
			base.ViewModelReady();
		
			var source = new SimpleTableViewSource<ReservationViewModel>(TableView, CellBindings.Reservations);
			TableView.Source = source;

			var set = this.CreateBindingSet<MyPageReservationsView, MyPageReservationsViewModel>();
			set.Bind(source).To(vm => vm.Reservations);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);
			set.Apply();

			TableView.ReloadData();

			_noRows.RemoveFromSuperview();

			if (ViewModel.Reservations.Count == 0)
			{
				_noRows = new UILabel(new RectangleF(10, 10, 300, 30)){ Text = "Du har ingen reservasjoner.", Font = Application.ThemeColors.DefaultFont };
				Add(_noRows);
			}
		}

		UILabel _noRows = new UILabel();
	}
	
}
