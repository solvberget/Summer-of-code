using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class HomeScreenViewModel : BaseViewModel
    {

        public HomeScreenViewModel()
        {
            Load();
        }

        private void Load()
        {
            ListElements = new List<HomeScreenElementViewModel>
            {
                new HomeScreenElementViewModel
                {
                    Title = "Min Side",
                    IconChar = "m",
                },
                new HomeScreenElementViewModel
                {
                    Title = "Arrangementer",
                    IconChar = "a"
                },
                new HomeScreenElementViewModel
                {
                    Title = "Søk",
                    IconChar = "s"
                },
                new HomeScreenElementViewModel
                {
                    Title = "Blogger",
                    IconChar = "e"
                },
                new HomeScreenElementViewModel
                {
                    Title = "Nyheter",
                    IconChar = "n"
                },
                new HomeScreenElementViewModel
                {
                    Title = "Anbefalinger",
                    IconChar = "h"
                },
                new HomeScreenElementViewModel
                {
                    Title = "Åpningstider",
                    IconChar = "å"
                },
                new HomeScreenElementViewModel
                {
                    Title = "Kontakt oss",
                    IconChar = "c"
                }
            };
        }

        public void Init()
        {
            Title = "Startside";
        }

        private List<HomeScreenElementViewModel> _listElements;
        public List<HomeScreenElementViewModel> ListElements 
        {
            get { return _listElements; }
            set { _listElements = value; RaisePropertyChanged(() => ListElements);}
        } 
    }
}
