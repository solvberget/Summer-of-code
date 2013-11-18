using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MenuViewModel : BaseViewModel 
    {
        private HomeViewModel.Section _section;
        public HomeViewModel.Section Section
        {
            get { return _section; }
            set
            {
                _section = value;
                Id = (int)_section; RaisePropertyChanged(() => Section);
            }
        }

        private string _iconChar;
        public string IconChar 
        {
            get { return _iconChar; }
            set { _iconChar = value; RaisePropertyChanged(() => IconChar);}
        }

        private bool _isSelected;
        public bool IsSelected 
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged(() => IsSelected);}
        }
    }
}
