using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.ViewModels
{
    public class MyPageFavoritesViewModel : BaseViewModel
    {
        public MyPageFavoritesViewModel()
        {
            var service = Mvx.Resolve<IUserInformationService>();
            if (service == null) throw new ArgumentNullException("service");

            Favorites = service.GetUserFavorites("id");
        }

        private List<Document> _favorites;
        public List<Document> Favorites
        {
            get{ return _favorites; }
            set{ _favorites = value; RaisePropertyChanged(() => Favorites); }
        }
    }
}
