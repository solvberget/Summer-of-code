using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class HomeScreenElementViewModel : BaseViewModel
    {
        private string _title;
        public string Title 
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

        private void ExecuteGoToCommand(HomeScreenElementViewModel element)
        {
            switch (Title)
            {
                case "Min Side":
                    ShowViewModel<MyPageViewModel>();
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
