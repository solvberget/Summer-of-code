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

		protected override void ViewModelReady()
		{
			base.ViewModelReady();
		
			var source = new SimpleTableViewSource<FineViewModel>(TableView, CellBindings.Fines);
			TableView.Source = source;

			var set = this.CreateBindingSet<MyPageFinesView, MyPageFinesViewModel>();
			set.Bind(source).To(vm => vm.Fines);

			set.Apply();

			TableView.ReloadData();

			_noRows.RemoveFromSuperview();

			if (ViewModel.Fines.Count == 0)
			{
				_noRows = new UILabel(new RectangleF(10, 10, 300, 30)){ Text = "Du har ingen gebyrer.", Font = Application.ThemeColors.DefaultFont };
				Add(_noRows);
			}
		}

		UILabel _noRows = new UILabel();
	}
	
}
