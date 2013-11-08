using System.Collections.Generic;
using System.Collections.ObjectModel;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPageFavoritesViewModel : BaseViewModel
    {
        private readonly IUserService _service;

        public MyPageFavoritesViewModel(IUserService service)
        {
            _service = service;
            Load();
        }

        private ObservableCollection<FavoriteViewModel> _favorites;
        public ObservableCollection<FavoriteViewModel> Favorites
        {
            get{ return _favorites; }
            set{ _favorites = value; RaisePropertyChanged(() => Favorites); }
        }

        public async void Load()
        {
            IsLoading = true;

            var favs = await _service.GetUserFavorites();

            //var user = await _service.GetUserInformation(_service.GetUserId());



            //Favorites = user..ToList();

            Favorites = new ObservableCollection<FavoriteViewModel>();

            foreach (FavoriteDto f in favs)
            {
                Favorites.Add(new FavoriteViewModel
                {
                    ButtonVisible = true,
                    Name = f.Document.Title,
                    Year = f.Document.Year,
                    Parent = this,
                    DocumentNumber = f.Document.Id
                });
            }

            if (Favorites.Count == 0)
            {
                Favorites.Add(new FavoriteViewModel
                {
                    Name = "Du har ingen registrerte favoritter",
                    ButtonVisible = false

                });
            }

            IsLoading = false;
        }

        public async void RemoveFavorite(string documentNumber, FavoriteViewModel favorite)
        {
            Favorites.Remove(favorite);
            await _service.RemoveUserFavorite(documentNumber);
            var hoppsidais = "";
        }

        public void AddFavorite(FavoriteViewModel favoriteViewModel)
        {
            Favorites.Add(favoriteViewModel);
        }
    }
}
