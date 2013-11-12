using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _service;
        private readonly IUserAuthenticationDataService _userAuthenticationService;

        public LoginViewModel(IUserService service, IUserAuthenticationDataService userAuthenticationDataService)
        {
            _service = service;
            _userAuthenticationService = userAuthenticationDataService;
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

        private bool _loggedIn;
        public bool LoggedIn
        {
            get { return _loggedIn; }
            set { _loggedIn = value; RaisePropertyChanged(() => LoggedIn); }
        }

        private MvxCommand<MyPageViewModel> _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new MvxCommand<MyPageViewModel>(ExecuteLoginCommand));
            }
        }

        private void ExecuteLoginCommand(MyPageViewModel page)
        {
            _userAuthenticationService.SetUser(UserName);
            _userAuthenticationService.SetPassword(Pin);

            var response = _service.Login(UserName, Pin).Result;

            if (response.Equals("Success"))
            {
                ShowViewModel<MyPageViewModel>();
            }
            else
            {
                //Popup: Login Failed!
                _userAuthenticationService.RemoveUser();
                _userAuthenticationService.RemovePassword();
            }
        }
    }
}
