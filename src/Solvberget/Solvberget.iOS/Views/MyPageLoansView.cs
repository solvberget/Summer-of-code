using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Solvberget.iOS
{

	public class MyPageLoansView : NamedTableViewController
	{
		public new MyPageLoansViewModel ViewModel
		{
			get
			{
				return base.ViewModel as MyPageLoansViewModel;
			}
		}

		protected override void ViewModelReady()
		{
			base.ViewModelReady();
		
			var source = new SimpleTableViewSource<LoanViewModel>(TableView, CellBindings.Loans);
			TableView.Source = source;

			var set = this.CreateBindingSet<MyPageLoansView, MyPageLoansViewModel>();
			set.Bind(source).To(vm => vm.Loans);

			source.SelectedItemChanged += (s,e) => ShowPopup(source.SelectedItem as LoanViewModel);

			set.Apply();

			TableView.ReloadData();

			_noRows.RemoveFromSuperview();

			if (ViewModel.Loans.Count == 0)
			{
				_noRows = new UILabel(new RectangleF(10, 10, 300, 30)){ Text = "Du har ingen lån.", Font = Application.ThemeColors.DefaultFont };
				Add(_noRows);
			}
		}

		void ShowPopup(LoanViewModel vm)
		{
			if (null == vm)
				return;

			var popup = new UIAlertView(View.Frame);

			popup.Title = vm.DocumentTitle;

			popup.AddButton("Utvid lånetid");
			popup.AddButton("Vis detaljer");
			popup.AddButton("Avbryt");

			popup.CancelButtonIndex = 2;

			popup.Dismissed += (sender, e) =>
			{
				switch(e.ButtonIndex)
				{
					case 0 : 
						ViewModel.ExpandLoan(vm.DocumentNumber);
						break;

					case 1: 
						ViewModel.ShowDetailsCommand.Execute(vm);
						break;
				}
			};

			popup.Show();
		}

		UILabel _noRows = new UILabel();
	}
	
}
