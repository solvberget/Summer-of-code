using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class HomeScreenElementViewModel : BaseViewModel
    {
        private readonly IUserAuthenticationDataService _userAuthenticationService;

        public HomeScreenElementViewModel(IUserAuthenticationDataService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        private string _title;
        public new string Title 
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(() => Title);}
        } 

        private string _iconChar;
        public string IconChar 
        {
            get { return _iconChar; }
            set { _iconChar = value; RaisePropertyChanged(() => IconChar);}
        } 

        private ICommand _goToCommand;

        public ICommand GoToCommand 
        {
            get { return _goToCommand ?? (_goToCommand = new MvxCommand<HomeScreenElementViewModel>(ExecuteGoToCommand)) ;
            }
        }

        public bool IsAuthenticated()
        {
            return _userAuthenticationService.UserInfoRegistered();
        }

        private string _badgeContent;
        public string BadgeContent 
        {
            get { return _badgeContent; }
            set { _badgeContent = value; RaisePropertyChanged(() => BadgeContent);}
        }

        private void ExecuteGoToCommand(HomeScreenElementViewModel element)
        {
            switch (Title)
            {
                case "Min Side":
                    if (IsAuthenticated())
                        ShowViewModel<MyPageViewModel>();
                    else
                        ShowViewModel<LoginViewModel>();
                    break;
                case "Arrangementer":
                    ShowViewModel<EventListViewModel>();
                    break;
                case "Søk":
                    ShowViewModel<SearchViewModel>();
                    break;
                case "Blogger":
                    ShowViewModel<BlogOverviewViewModel>();
                    break;
                case "Nyheter":
                    ShowViewModel<NewsListingViewModel>();
                    break;
                case "Anbefalinger":
                    ShowViewModel<SuggestionsListListViewModel>();
                    break;
                case "Åpningstider":
                    ShowViewModel<OpeningHoursViewModel>();
                    break;
                case "Kontakt oss":
                    ShowViewModel<ContactInfoViewModel>();
                    break;
                default:
                    ShowViewModel<HomeScreenViewModel>();
                    break;
            }
        }
    }
}
