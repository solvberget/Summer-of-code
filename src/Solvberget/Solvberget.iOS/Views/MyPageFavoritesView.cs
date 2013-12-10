using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Solvberget.iOS
{

	public partial class MyPageFavoritesView : NamedTableViewController
    {
		public new MyPageFavoritesViewModel ViewModel
		{
			get
			{
				return base.ViewModel as MyPageFavoritesViewModel;
			}
		}

		protected override void ViewModelReady()
		{
			base.ViewModelReady();
		
			var source = new SimpleTableViewSource<FavoriteViewModel>(TableView, CellBindings.Favorites);
			TableView.Source = source;

			var set = this.CreateBindingSet<MyPageFavoritesView, MyPageFavoritesViewModel>();
			set.Bind(source).To(vm => vm.Favorites);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);

			set.Apply();

			TableView.ReloadData();
		
			_noRows.RemoveFromSuperview();

			if (ViewModel.Favorites.Count == 0)
			{
				_noRows = new UILabel(new RectangleF(10, 10, 300, 30)){ Text = "Du har ingen favoritter.", Font = Application.ThemeColors.DefaultFont };
				Add(_noRows);
			}
		}

		UILabel _noRows = new UILabel();

    }
}

