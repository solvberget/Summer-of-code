using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Solvberget.iOS
{

	public class MyPageFinesView : NamedTableViewController
	{
		public new MyPageFinesViewModel ViewModel
		{
			get
			{
				return base.ViewModel as MyPageFinesViewModel;
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var source = new SimpleTableViewSource<FineViewModel>(TableView, CellBindings.Fines);
			TableView.Source = source;

			var loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);

			var set = this.CreateBindingSet<MyPageFinesView, MyPageFinesViewModel>();
			set.Bind(source).To(vm => vm.Fines);

			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			ViewModel.WaitForReady(() => InvokeOnMainThread(OnViewModelReady));

			TableView.ReloadData();
		}

		private void OnViewModelReady()
		{
			if (ViewModel.Fines.Count == 0)
			{
				Add(new UILabel(new RectangleF(10, 10, 300, 30)){ Text = "Du har ingen gebyrer.", Font = Application.ThemeColors.DefaultFont });
			}
		}

	}
	
}
