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
            this.m_MenuItems = new List<MenuViewModel>
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

        private List<MenuViewModel> m_MenuItems;
        public List<MenuViewModel> MenuItems
        {
            get { return this.m_MenuItems; }
            set { this.m_MenuItems = value; this.RaisePropertyChanged(() => this.MenuItems); }
        }

        private MvxCommand<MenuViewModel> m_SelectMenuItemCommand;
        public ICommand SelectMenuItemCommand
        {
            get
            {
                return this.m_SelectMenuItemCommand ?? (this.m_SelectMenuItemCommand = new MvxCommand<MenuViewModel>(this.ExecuteSelectMenuItemCommand));
            }
        }

        private void ExecuteSelectMenuItemCommand(MenuViewModel item)
        {
            //navigate if we have to, pass the id so we can grab from cache... or not
            switch (item.Section)
            {

                case Section.MyPage:
                    //this.ShowViewModel<MyPageViewModel>();
                    break;
                case Section.Search:
                    //this.ShowViewModel<MyPageViewModel>();
                    break;
                case Section.Lists:
                    //this.ShowViewModel<MyPageViewModel>();
                    break;
                case Section.Events:
                    //this.ShowViewModel<MyPageViewModel>();
                    break;
                case Section.Blogs:
                    //this.ShowViewModel<MyPageViewModel>();
                    break;
                case Section.News:
                    //this.ShowViewModel<MyPageViewModel>();
                    break;
                case Section.OpeningHours:
                    //this.ShowViewModel<MyPageViewModel>();
                    break;
                case Section.Contact:
                    //this.ShowViewModel<MyPageViewModel>();
                    break;
            }
        }

        public Section GetSectionForViewModelType(Type type)
        {

            if (type == typeof(MyPageViewModel))
                return Section.MyPage;

            return Section.Unknown;
        }
    }
}
