using System;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class DocumentAvailabilityViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private BaseViewModel _parent;

        public DocumentAvailabilityViewModel(IUserService userService, BaseViewModel parent)
        {
            _userService = userService;
            _parent = parent;
        }


        private string _docId;
        public string DocId 
        {
            get { return _docId; }
            set { _docId = value; RaisePropertyChanged(() => DocId);}
        }

        private string _branch;
        public string Branch 
        {
            get { return _branch; }
            set { _branch = value; RaisePropertyChanged(() => Branch);}
        }

        private string _department;
        public string Department 
        {
            get { return _department; }
            set { _department = value; RaisePropertyChanged(() => Department);}
        }

        private string _collection;
        public string Collection 
        {
            get { return _collection; }
            set { _collection = value; RaisePropertyChanged(() => Collection);}
        }

        private string _location;
        public string Location 
        {
            get { return _location; }
            set { _location = value; RaisePropertyChanged(() => Location);}
        }   

        private int _availableCount;
        public int AvailableCount 
        {
            get { return _availableCount; }
            set { _availableCount = value; RaisePropertyChanged(() => AvailableCount);}
        }

        private int _totalCount;
        public int TotalCount 
        {
            get { return _totalCount; }
            set { _totalCount = value; RaisePropertyChanged(() => TotalCount);}
        }

        private DateTime? _estimatedAvailableDate;
        public DateTime? EstimatedAvailableDate 
        {
            get { return _estimatedAvailableDate; }
            set { _estimatedAvailableDate = value; RaisePropertyChanged(() => EstimatedAvailableDate);}
        }

        private string _availabilitySummary;
        public string AvailabilitySummary
        {
            get
            {
                return string.Format("{0} av {1} tilgjengelig", AvailableCount, TotalCount);
            }
        }

        private MvxCommand _placeHoldRequestCommand;
        public ICommand PlaceHoldRequestCommand
        {
            get
            {
                return _placeHoldRequestCommand ??
                       (_placeHoldRequestCommand = new MvxCommand(ExecutePlaceHoldRequestCommand));
            }
        }

        private async void ExecutePlaceHoldRequestCommand()
        {
            _parent.IsLoading = true;
            await _userService.AddReservation(DocId, Branch);
            _parent.IsLoading = false;

            IsDirty = true;
        }

        private bool _isReservable;
        public bool IsReservable
        {
            get { return _isReservable; }
            set { _isReservable = value; RaisePropertyChanged(() => IsReservable); }
        }

        private string _buttonText;
        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; RaisePropertyChanged(() => ButtonText); }
        }

        private bool _isDirty;
        public bool IsDirty 
        {
            get { return _isDirty; }
            set { _isDirty = value; RaisePropertyChanged(() => IsDirty);}
        }
    }
}