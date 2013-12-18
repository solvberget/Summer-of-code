using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IUserAuthenticationDataService _userAuthenticationService;

        public enum Section
        {
            MyPage,
            Search,
            Lists,
            Events,
            Blogs,
            News,
            OpeningHours,
            Contact,
            Home,
            Logout,
            Unknown
        }

        public HomeViewModel(IUserAuthenticationDataService userAuthenticationDataService)
        {
            _userAuthenticationService = userAuthenticationDataService;
            _menuItems = new List<MenuViewModel>
                {
                    new MenuViewModel
                        {
                            Section = Section.Home,
                            Title = "Hjem",
                            IconChar = "1",
                            ViewModelType = typeof (HomeScreenViewModel)
                        },
                    new MenuViewModel
                        {
                            Section = Section.MyPage,
                            Title = "Min Side",
                            IconChar = "m",
                            ViewModelType = typeof (MyPageViewModel)
                        },
                    new MenuViewModel
                        {
                            Section = Section.Events,
                            Title = "Arrangementer",
                            IconChar = "a",
                            ViewModelType = typeof (EventListViewModel)
                        },
                    new MenuViewModel
                        {
                            Section = Section.Search,
                            Title = "Søk",
                            IconChar = "s",
                            ViewModelType = typeof (SearchViewModel)
                        },
                    new MenuViewModel
                        {
                            Section = Section.Blogs,
                            Title = "Blogger",
                            IconChar = "e",
                            ViewModelType = typeof (BlogOverviewViewModel)
                        },
                    new MenuViewModel
                        {
                            Section = Section.News,
                            Title = "Nyheter",
                            IconChar = "n",
                            ViewModelType = typeof (NewsListingViewModel)
                        },
                    new MenuViewModel
                        {
                            Section = Section.Lists,
                            Title = "Anbefalinger",
                            IconChar = "h",
                            ViewModelType = typeof (SuggestionsListListViewModel)
                        },
                    new MenuViewModel
                        {
                            Section = Section.OpeningHours,
                            Title = "Åpningstider",
                            IconChar = "å",
                            ViewModelType = typeof (OpeningHoursViewModel)
                        },
                    new MenuViewModel
                        {
                            Section = Section.Contact,
                            Title = "Kontakt oss",
                            IconChar = "c",
                            ViewModelType = typeof (ContactInfoViewModel)
                        },
                    new MenuViewModel
                        {
                            Section = Section.Logout,
                            Title = "Logg ut",
                            IconChar = "2",
                        }
                };

            LoggedIn = IsAuthenticated();
        }

        private List<MenuViewModel> _menuItems;

        public List<MenuViewModel> MenuItems
        {
            get
            {
                if (LoggedIn)
                    return _menuItems;

                return _menuItems.Where(mi => mi.Section != Section.Logout).ToList();
            }
            set
            {
                _menuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }

        private MvxCommand<MenuViewModel> _selectMenuItemCommand;

        public ICommand SelectMenuItemCommand
        {
            get
            {
                return _selectMenuItemCommand ??
                       (_selectMenuItemCommand = new MvxCommand<MenuViewModel>(ExecuteSelectMenuItemCommand));
            }
        }

        private bool _loggedIn;

        public bool LoggedIn
        {
            get { return _loggedIn; }
            set
            {
                _loggedIn = value;
                RaisePropertyChanged(() => LoggedIn);
                RaisePropertyChanged(() => MenuItems);
            }
        }

        private void ExecuteSelectMenuItemCommand(MenuViewModel item)
        {
            //navigate if we have to, pass the id so we can grab from cache... or not
            switch (item.Section)
            {
                case Section.MyPage:
                    if (_userAuthenticationService.GetUserId().Equals("Fant ikke brukerid"))
                        ShowViewModel<LoginViewModel>();
                    else
                        ShowViewModel<MyPageViewModel>();
                    break;
                case Section.Home:
                    ShowViewModel<HomeScreenViewModel>();
                    break;
                case Section.Search:
                    ShowViewModel<SearchViewModel>();
                    break;
                case Section.Lists:
                    ShowViewModel<SuggestionsListListViewModel>();
                    break;
                case Section.Events:
                    ShowViewModel<EventListViewModel>();
                    break;
                case Section.Blogs:
                    ShowViewModel<BlogOverviewViewModel>();
                    break;
                case Section.News:
                    ShowViewModel<NewsListingViewModel>();
                    break;
                case Section.OpeningHours:
                    ShowViewModel<OpeningHoursViewModel>();
                    break;
                case Section.Contact:
                    ShowViewModel<ContactInfoViewModel>();
                    break;
                case Section.Logout:
                    LogOut();

                    ShowViewModel<HomeScreenViewModel>();
                    break;
            }
        }

        public Section GetSectionForViewModelType(Type type)
        {
            if (type == typeof (MyPageViewModel))
                return Section.MyPage;
            if (type == typeof (HomeScreenViewModel))
                return Section.Home;
            if (type == typeof (LoginViewModel))
                return Section.Unknown;
            if (type == typeof (SearchViewModel))
                return Section.Search;
            if (type == typeof (NewsListingViewModel))
                return Section.News;
            if (type == typeof (OpeningHoursViewModel))
                return Section.OpeningHours;
            if (type == typeof (SuggestionsListListViewModel))
                return Section.Lists;
            if (type == typeof (ContactInfoViewModel))
                return Section.Contact;
            if (type == typeof (BlogOverviewViewModel))
                return Section.Blogs;
            if (type == typeof (EventListViewModel))
                return Section.Events;

            return Section.Unknown;
        }

        public bool IsAuthenticated()
        {
            return _userAuthenticationService.UserInfoRegistered();
        }

        public void LogOut()
        {
            _userAuthenticationService.RemoveUser();
            _userAuthenticationService.RemovePassword();
            LoggedIn = false;
        }
    }
}
