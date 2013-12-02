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

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(() => Message); }
        }

        private bool _buttonPressed;
        public bool ButtonPressed 
        {
            get { return _buttonPressed; }
            set { _buttonPressed = value; RaisePropertyChanged(() => ButtonPressed);}
        } 

        private MvxCommand<MyPageViewModel> _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new MvxCommand<MyPageViewModel>(ExecuteLoginCommand));
            }
        }

        private async void ExecuteLoginCommand(MyPageViewModel page)
        {
            IsLoading = true;

            ButtonPressed = true;
            _userAuthenticationService.SetUser(UserName);
            _userAuthenticationService.SetPassword(Pin);

            var response = await _service.Login(UserName, Pin);
            IsLoading = false;

            if (response.Message.Equals("Autentisering vellykket."))
            {
                ShowViewModel<MyPageViewModel>();
            }
            else if (response.Message.Equals("The remote server returned an error: (401) Unauthorized."))
            {
                Message = "Feil brukernavn eller passord";
                _userAuthenticationService.RemoveUser();
                _userAuthenticationService.RemovePassword();
            }
            else
            {
                Message = "Noe gikk galt. Prøv igjen senere";
                _userAuthenticationService.RemoveUser();
                _userAuthenticationService.RemovePassword();
            }
        }
    }
}
