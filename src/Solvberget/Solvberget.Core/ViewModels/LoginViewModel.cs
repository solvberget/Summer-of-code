using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _service;

        public LoginViewModel(IUserService service)
        {
            _service = service;
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged(() => UserName); }
        }

        private string _pin;
        public string Pin
        {
            get { return _pin; }
            set { _pin = value; RaisePropertyChanged(() => Pin); }
        }

        
    }
}
