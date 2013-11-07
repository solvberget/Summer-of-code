using System.Collections.Generic;
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

        private List<FavoriteViewModel> _favorites;
        public List<FavoriteViewModel> Favorites
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

            Favorites = new List<FavoriteViewModel>();

            foreach (FavoriteDto f in favs)
            {
                Favorites.Add(new FavoriteViewModel
                {
                    ButtonVisible = true,
                    Name = f.Document.Title,
                    Year = f.Document.Year,
                    Parent = this
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

        public void RemoveFavorite(FavoriteViewModel favoriteViewModel)
        {
            Favorites.Remove(favoriteViewModel);
        }

        public void AddFavorite(FavoriteViewModel favoriteViewModel)
        {
            Favorites.Add(favoriteViewModel);
        }
    }
}
