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

        private List<DocumentDto> _favorites;
        public List<DocumentDto> Favorites
        {
            get{ return _favorites; }
            set{ _favorites = value; RaisePropertyChanged(() => Favorites); }
        }

        public async void Load()
        {
            IsLoading = true;

            var user = await _service.GetUserInformation(_service.GetUserId());

            //Favorites = user..ToList();

            IsLoading = false;
        }
    }
}
