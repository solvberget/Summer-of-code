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

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var source = new SimpleTableViewSource<LoanViewModel>(TableView, CellBindings.Loans);
			TableView.Source = source;

			var loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);

			var set = this.CreateBindingSet<MyPageLoansView, MyPageLoansViewModel>();
			set.Bind(source).To(vm => vm.Loans);
			//set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);

			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			ViewModel.WaitForReady(() => InvokeOnMainThread(OnViewModelReady));

			TableView.ReloadData();
		}

		private void OnViewModelReady()
		{
			if (ViewModel.Loans.Count == 0)
			{
				Add(new UILabel(new RectangleF(10, 10, 300, 30)){ Text = "Du har ingen lån.", Font = Application.ThemeColors.DefaultFont });
			}
		}
	}
	
}
