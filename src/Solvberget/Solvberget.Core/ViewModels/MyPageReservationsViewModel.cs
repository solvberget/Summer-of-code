using System;
using System.Collections.Generic;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPageReservationsViewModel : BaseViewModel
    {
        public MyPageReservationsViewModel(IUserService service)
        {
            if (service == null) throw new ArgumentNullException("service");

            //Reservations = service.GetUserReservations("id");
        }

        private List<Reservation> _reservations;
        public List<Reservation> Reservations
        {
            get{ return _reservations; }
            set{ _reservations = value; RaisePropertyChanged(() => Reservations); }
        }
    }
}
