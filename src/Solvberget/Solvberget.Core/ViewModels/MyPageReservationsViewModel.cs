using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using System.Linq;


namespace Solvberget.Core.ViewModels
{
    public class MyPageReservationsViewModel : BaseViewModel
    {
        private readonly IUserService _service;
        private readonly IUserAuthenticationDataService _userAuthenticationService;

        public MyPageReservationsViewModel(IUserService service, IUserAuthenticationDataService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
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
            var res = await _service.GetUerReservations() ?? new List<ReservationDto>();

            Reservations = new ObservableCollection<ReservationViewModel>();

            foreach (ReservationDto r in res)
            {
                Reservations.Add(new ReservationViewModel
                {
                    DocumentTitle = r.Document.Title,
                    DocumentNumber = r.Document.Id,
                    HoldRequestFrom = r.Reserved,
                    ReadyForPickup = r.ReadyForPickup,
                    PickupLocation = r.PickupLocation.Equals("Hovedbibl.") ? "Hovedbiblioteket" : r.PickupLocation,
                    Parent = this,
                    ButtonVisible = true,
                    CancellationButtonVisible = false,
                    ButtonText = "Fjern",
                    Status = r.ReadyForPickup ? "" : "Ikke klar for henting"
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

        public async void RemoveReservation(ReservationViewModel reservationViewModel)
        {
            Reservations.Remove(reservationViewModel);

            var response = await _service.RemoveReservation(reservationViewModel.DocumentNumber);

            var bomtibom = "hoi";
        }

        public void AddReservation(ReservationViewModel reservationViewModel)
        {
            Reservations.Add(reservationViewModel);
        }
    }
}
