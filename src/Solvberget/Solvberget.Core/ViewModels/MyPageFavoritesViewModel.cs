using System.Collections.ObjectModel;
using System.Linq;
using Solvberget.Core.DTOs;
using Solvberget.Core.Properties;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;

namespace Solvberget.Core.ViewModels
{
    public class MyPageFavoritesViewModel : BaseViewModel
    {
        private readonly IUserService _service;

        public MyPageFavoritesViewModel(IUserService service)
        {
			Title = "Favoritter";
            _service = service;
        }

        private ObservableCollection<FavoriteViewModel> _favorites;
        public ObservableCollection<FavoriteViewModel> Favorites
        {
            get{ return _favorites; }
            set{ _favorites = value; RaisePropertyChanged(() => Favorites); }
        }

        private bool _favoriteIsRemoved;
        public bool FavoriteIsRemoved
        {
            get { return _favoriteIsRemoved; }
            set { _favoriteIsRemoved = value; RaisePropertyChanged(() => FavoriteIsRemoved); }
        }

		public override void OnViewReady()
		{
			base.OnViewReady();

			Load();
		}

        public async void Load()
        {
            IsLoading = true;

            var favs = await _service.GetUserFavorites();

            Favorites = new ObservableCollection<FavoriteViewModel>();

            foreach (FavoriteDto f in favs)
            {
                Favorites.Add(new FavoriteViewModel
                {
                    ButtonVisible = true,
                    Name = f.Document.Title,
                    Year = f.Document.Year,
                    Type = f.Document.Type,
                    Parent = this,
                    DocumentNumber = f.Document.Id,
                    Image = Resources.ServiceUrl + string.Format(Resources.ServiceUrl_MediaImage, f.Document.Id)
                });
            }

			if (Favorites.Count == 0 && AddEmptyItemForEmptyLists)
            {
                Favorites.Add(new FavoriteViewModel
                {
                    Name = "Du har ingen registrerte favoritter",
                    ButtonVisible = false

                });
            }

			IsLoading = false;
			NotifyViewModelReady();
        }

        public async void RemoveFavorite(string documentNumber, FavoriteViewModel favorite)
        {
            Favorites.Remove(favorite);
            FavoriteIsRemoved = true;
            await _service.RemoveUserFavorite(documentNumber);
        }

        public void AddFavorite(FavoriteViewModel favoriteViewModel)
        {
            Favorites.Add(favoriteViewModel);
        }

		private MvxCommand<FavoriteViewModel> _showDetailsCommand;
		public ICommand ShowDetailsCommand
		{
			get
			{
				return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<FavoriteViewModel>(ExecuteShowDetailsCommand));
			}
		}

		private void ExecuteShowDetailsCommand(FavoriteViewModel model)
		{
			if (model.DocumentNumber != "")
			{
				ShowViewModel<MediaDetailViewModel>(new { title = model.Name, docId = model.DocumentNumber });
			}
		}
    }
}
