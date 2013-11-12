using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPagePersonaliaViewModel : BaseViewModel
    {
        private readonly IUserService _service;
        private readonly IUserAuthenticationDataService _userAuthenticationService;

        public MyPagePersonaliaViewModel(IUserService service, IUserAuthenticationDataService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
            _service = service;
            Load();
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }
        private string _streetAdress;
        public string StreetAdress
        {
            get
            {
                return _streetAdress;
            }
            set
            {
                _streetAdress = value;
                RaisePropertyChanged(() => StreetAdress);
            }
        }

        private string _cityAdress;
        public string CityAdress
        {
            get
            {
                return _cityAdress;
            }
            set
            {
                _cityAdress = value;
                RaisePropertyChanged(() => CityAdress);
            }
        }

        private string _cellPhoneNumber;
        public string CellPhoneNumber
        {
            get
            {
                return _cellPhoneNumber;
            }
            set
            {
                _cellPhoneNumber = value;
                RaisePropertyChanged(() => CellPhoneNumber);
            }
        }

        private string _dateOfBirth;
        public string DateOfBirth
        {
            get
            {
                return _dateOfBirth;
            }
            set
            {
                _dateOfBirth = value;
                RaisePropertyChanged(() => DateOfBirth);
            }
        }

        public async void Load()
        {
            var user = await _service.GetUserInformation(_userAuthenticationService.GetUserId());

            DateOfBirth = user.DateOfBirth;
            CellPhoneNumber = user.CellPhoneNumber;
            CityAdress = user.CityAddress;
            StreetAdress = user.StreetAddress;
            Email = user.Email;
            Name = user.Name;
        }
    }
}
