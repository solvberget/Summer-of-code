using System;
using Cirrious.CrossCore;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPagePersonaliaViewModel : BaseViewModel
    {
        public MyPagePersonaliaViewModel()
        {
            var service = Mvx.Resolve<IUserInformationService>();
            if (service == null) throw new ArgumentNullException("service");

            var user = service.GetUserInformation("id");
            Name = user.Name;
            Email = user.Email;
            StreetAdress = user.StreetAddress;
            CityAdress = user.CityAddress;
            CellPhoneNumber = user.CellPhoneNumber;
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
    }
}
