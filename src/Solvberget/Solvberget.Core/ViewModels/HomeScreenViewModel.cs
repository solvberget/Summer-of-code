using System.Collections.Generic;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using System.Linq;

namespace Solvberget.Core.ViewModels
{
    public class HomeScreenViewModel : BaseViewModel
    {
        private const string MY_PAGE_TITLE = "Min Side";
        private readonly IUserAuthenticationDataService _userAuthenticationService;
		IUserService _users;

		public HomeScreenViewModel(IUserAuthenticationDataService userAuthenticationDataService, IUserService users)
        {
            _userAuthenticationService = userAuthenticationDataService;
			_users = users;
            Load();
        }

		private async void Load()
        {
		    ListElements = new List<HomeScreenElementViewModel>
            {
                new HomeScreenElementViewModel(_userAuthenticationService)
                {
                    Title = MY_PAGE_TITLE,
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

            var user = await _users.GetUserInformation(true);
            MyPageBadgeText = null == user || null == user.Notifications ? string.Empty : user.Notifications.Count().ToString();
            var myPageElement = ListElements.SingleOrDefault(le => le.Title == MY_PAGE_TITLE);
            if (myPageElement != null) myPageElement.BadgeContent = MyPageBadgeText;

		    NotifyViewModelReady();
        }

        public void Init()
        {
			Title = "Sølvberget";
        }

		string _myPageBadgeText;
		public string MyPageBadgeText
		{
			get
			{
				return _myPageBadgeText;
			}
			set
			{
				_myPageBadgeText = value;
				RaisePropertyChanged(() => MyPageBadgeText);
			}
		}

        private List<HomeScreenElementViewModel> _listElements;
        public List<HomeScreenElementViewModel> ListElements 
        {
            get { return _listElements; }
            set { _listElements = value; RaisePropertyChanged(() => ListElements);}
        } 
    }
}
