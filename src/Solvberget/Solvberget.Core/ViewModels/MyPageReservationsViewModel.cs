using System.Collections.Generic;
using System.Collections.ObjectModel;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using System.Linq;


namespace Solvberget.Core.ViewModels
{
    public class MyPageReservationsViewModel : BaseViewModel
    {
        private readonly IUserService _service;

        public MyPageReservationsViewModel(IUserService service)
        {
            _service = service;
            Load();
        }

        private ObservableCollection<ReservationViewModel> _reservations;
        public ObservableCollection<ReservationViewModel> Reservations
        {
            get{ return _reservations; }
            set{ _reservations = value; RaisePropertyChanged(() => Reservations); }
        }


        public async void Load()
        {
            var user = await _service.GetUserInformation(_service.GetUserId());

            var res = user.Reservations == null ? new List<ReservationDto>() : user.Reservations.ToList();

            Reservations = new ObservableCollection<ReservationViewModel>();

            foreach (ReservationDto r in res)
            {
                Reservations.Add(new ReservationViewModel
                {
                    DocumentTitle = r.DocumentTitle,
                    DocumentNumber = r.DocumentNumber,
                    HoldRequestFrom = r.HoldRequestFrom,
                    Status = r.Status,
                    PickupLocation = r.PickupLocation,
                    Parent = this,
                    ButtonVisible = true
                });
            }

            if (Reservations.Count == 0)
            {
                Reservations.Add(new ReservationViewModel
                {
                    DocumentTitle = "Du har ingen reservasjoner",
                    Status = "Du kan reservere gjennom mediedetaljsiden, enten gjennom søkeresultater, eller anbefalingslistene.",
                    ButtonVisible = false

                });
            }
        }

        public void RemoveReservation(ReservationViewModel reservationViewModel)
        {
            Reservations.Remove(reservationViewModel);
        }

        public void AddReservation(ReservationViewModel reservationViewModel)
        {
            Reservations.Add(reservationViewModel);
        }
    }
}
