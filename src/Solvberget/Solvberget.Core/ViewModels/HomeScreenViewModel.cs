using System.Collections.Generic;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class HomeScreenViewModel : BaseViewModel
    {
        private readonly IUserAuthenticationDataService _userAuthenticationService;

        public HomeScreenViewModel(IUserAuthenticationDataService userAuthenticationDataService)
        {
            _userAuthenticationService = userAuthenticationDataService;
            Load();
        }

        private void Load()
        {
            ListElements = new List<HomeScreenElementViewModel>
            {
                new HomeScreenElementViewModel(_userAuthenticationService)
                {
                    Title = "Min Side",
                    IconChar = "m",
                },
                new HomeScreenElementViewModel(_userAuthenticationService)
                {
                    Title = "Arrangementer",
                    IconChar = "a"
                },
                new HomeScreenElementViewModel(_userAuthenticationService)
                {
                    Title = "Søk",
                    IconChar = "s"
                },
                new HomeScreenElementViewModel(_userAuthenticationService)
                {
                    Title = "Blogger",
                    IconChar = "e"
                },
                new HomeScreenElementViewModel(_userAuthenticationService)
                {
                    Title = "Nyheter",
                    IconChar = "n"
                },
                new HomeScreenElementViewModel(_userAuthenticationService)
                {
                    Title = "Anbefalinger",
                    IconChar = "h"
                },
                new HomeScreenElementViewModel(_userAuthenticationService)
                {
                    Title = "Åpningstider",
                    IconChar = "å"
                },
                new HomeScreenElementViewModel(_userAuthenticationService)
                {
                    Title = "Kontakt oss",
                    IconChar = "c"
                }
            };
        }

        public void Init()
        {
			Title = "Sølvberget";
        }

        private List<HomeScreenElementViewModel> _listElements;
        public List<HomeScreenElementViewModel> ListElements 
        {
            get { return _listElements; }
            set { _listElements = value; RaisePropertyChanged(() => ListElements);}
        } 
    }
}
