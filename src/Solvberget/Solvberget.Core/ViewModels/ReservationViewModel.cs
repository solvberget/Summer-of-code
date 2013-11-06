using System;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.DTOs;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class ReservationViewModel : BaseViewModel
    {
        private string _documentNumber;
        public string DocumentNumber
        {
            get { return _documentNumber; }
            set
            {
                _documentNumber = value;
                RaisePropertyChanged(() => DocumentNumber);
            }
        }

        private string _documentTitle;
        public string DocumentTitle
        {
            get { return _documentTitle; }
            set
            {
                _documentTitle = value;
                RaisePropertyChanged(() => DocumentTitle);
            }
        }

        private string _pickupLocation;
        public string PickupLocation
        {
            get { return _pickupLocation; }
            set
            {
                _pickupLocation = value;
                RaisePropertyChanged(() => PickupLocation);
            }
        }

        private DateTime? _holdRequestFrom;
        public DateTime? HoldRequestFrom
        {
            get { return _holdRequestFrom; }
            set
            {
                _holdRequestFrom = value;
                RaisePropertyChanged(() => HoldRequestFrom);
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        private MyPageReservationsViewModel _parent;
        public MyPageReservationsViewModel Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                RaisePropertyChanged(() => Parent);
            }
        }

        private bool _buttonVisible;
        public bool ButtonVisible
        {
            get { return _buttonVisible; }
            set
            {
                _buttonVisible = value;
                RaisePropertyChanged(() => ButtonVisible);
            }
        }

        private MvxCommand<ReservationViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<ReservationViewModel>(ExecuteShowDetailsCommand));
            }
        }

        private void ExecuteShowDetailsCommand(ReservationViewModel reservation)
        {
            Parent.RemoveReservation(this);

            if (Parent.Reservations.Count == 0)
            {
                Parent.AddReservation(new ReservationViewModel
                {
                    DocumentTitle = "Du har ingen reservasjoner",
                    Status = "Du kan reservere gjennom mediedetaljsiden, enten gjennom søkeresultater, eller anbefalingslistene.",
                    ButtonVisible = false

                });
            }
        }
    }
}
