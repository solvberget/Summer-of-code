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


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var source = new SimpleTableViewSource<FavoriteViewModel>(TableView, CellBindings.Favorites);
			TableView.Source = source;

			var loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);

			var set = this.CreateBindingSet<MyPageFavoritesView, MyPageFavoritesViewModel>();
			set.Bind(source).To(vm => vm.Favorites);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);

			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			ViewModel.WaitForReady(() => InvokeOnMainThread(OnViewModelReady));

			TableView.ReloadData();
		}

		private void OnViewModelReady()
		{
			if (ViewModel.Favorites.Count == 0)
			{
				Add(new UILabel(new RectangleF(10, 10, 300, 30)){ Text = "Du har ingen favoritter.", Font = Application.ThemeColors.DefaultFont });
			}
		}

    }
}

