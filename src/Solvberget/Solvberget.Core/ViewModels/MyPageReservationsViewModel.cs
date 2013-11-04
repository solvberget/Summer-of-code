using System.Collections.Generic;
using System.Linq;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

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

        private List<ReservationDto> _reservations;
        public List<ReservationDto> Reservations
        {
            get{ return _reservations; }
            set{ _reservations = value; RaisePropertyChanged(() => Reservations); }
        }

        public async void Load()
        {
            var user = await _service.GetUserInformation(_service.GetUserId());

            Reservations = user.Reservations == null ? new List<ReservationDto>() : user.Reservations.ToList();

            if (Reservations.Count == 0)
            {
                Reservations = new List<ReservationDto>
                {
                    new ReservationDto
                    {
                        DocumentTitle = "Ingen reservasjoner. Du kan reservere gjennom mediedetaljsiden, enten gjennom søkeresultater, eller anbefalingslistene."
                    }
                };
            }
        }
    }
}
