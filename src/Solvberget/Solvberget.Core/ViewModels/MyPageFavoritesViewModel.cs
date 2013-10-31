using System;
using System.Collections.Generic;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPageFavoritesViewModel : BaseViewModel
    {
        public MyPageFavoritesViewModel(IUserService service)
        {
            if (service == null) throw new ArgumentNullException("service");

            //Favorites = service.GetUserFavorites("id");
        }

        private List<Document> _favorites;
        public List<Document> Favorites
        {
            get{ return _favorites; }
            set{ _favorites = value; RaisePropertyChanged(() => Favorites); }
        }
    }
}
