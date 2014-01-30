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

			source.SelectedItemChanged += (s,e) => ShowPopup(source.SelectedItem as FavoriteViewModel);

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

		void ShowPopup(FavoriteViewModel vm)
		{
			if (null == vm)
				return;

			var popup = new UIAlertView(View.Frame);

			popup.Title = vm.Name;

			popup.AddButton("Fjern fra favoritter");
			popup.AddButton("Vis detaljer");
			popup.AddButton("Avbryt");

			popup.CancelButtonIndex = 2;

			popup.Dismissed += (sender, e) =>
			{
				switch(e.ButtonIndex)
				{
					case 0 : 
						ViewModel.RemoveFavorite(vm.DocumentNumber, vm);
						break;

					case 1: 
						ViewModel.ShowDetailsCommand.Execute(vm);
						break;
				}
			};

			popup.Show();
		}
    }

}

