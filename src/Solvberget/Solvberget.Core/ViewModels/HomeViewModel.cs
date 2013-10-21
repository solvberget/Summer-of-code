using System;
using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
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
            Unknown
        }

        public HomeViewModel()
        {
            _menuItems = new List<MenuViewModel>
                              {
                                  new MenuViewModel
                                      {
                                          Section = Section.MyPage,
                                          Title = "Min Side"
                                      },
                                  new MenuViewModel
                                      {
                                          Section = Section.Search,
                                          Title = "Søk"
                                      },
                                  new MenuViewModel
                                      {
                                          Section = Section.Lists,
                                          Title = "Anbefalinger"
                                      },
                                 new MenuViewModel
                                      {
                                          Section = Section.Events,
                                          Title = "Arrangementer"
                                      },
                                  new MenuViewModel
                                      {
                                          Section = Section.Blogs,
                                          Title = "Blogger"
                                      },
                                  new MenuViewModel
                                      {
                                          Section = Section.News,
                                          Title = "Nyheter"
                                      },
                                  new MenuViewModel
                                      {
                                          Section = Section.OpeningHours,
                                          Title = "Åpningstider"
                                      },
                                  new MenuViewModel
                                      {
                                          Section = Section.Contact,
                                          Title = "Kontakt oss"
                                      },

                              };
        }

        private List<MenuViewModel> _menuItems;
        public List<MenuViewModel> MenuItems
        {
            get { return _menuItems; }
            set { _menuItems = value; RaisePropertyChanged(() => MenuItems); }
        }

        private MvxCommand<MenuViewModel> _selectMenuItemCommand;
        public ICommand SelectMenuItemCommand
        {
            get
            {
                return _selectMenuItemCommand ?? (_selectMenuItemCommand = new MvxCommand<MenuViewModel>(ExecuteSelectMenuItemCommand));
            }
        }

        private void ExecuteSelectMenuItemCommand(MenuViewModel item)
        {
            //navigate if we have to, pass the id so we can grab from cache... or not
            switch (item.Section)
            {

                case Section.MyPage:
                    ShowViewModel<MyPageViewModel>(item.Id);
                    break;
                case Section.Search:
                    ShowViewModel<SearchViewModel>(item.Id);
                    break;
                case Section.Lists:
                    break;
                case Section.Events:
                    break;
                case Section.Blogs:
                    break;
                case Section.News:
                    break;
                case Section.OpeningHours:
                    break;
                case Section.Contact:
                    break;
            }
        }

        public Section GetSectionForViewModelType(Type type)
        {

            if (type == typeof(MyPageViewModel))
                return Section.MyPage;
            if (type == typeof(SearchViewModel))
                return Section.Search;

            return Section.Unknown;
        }
    }
}
