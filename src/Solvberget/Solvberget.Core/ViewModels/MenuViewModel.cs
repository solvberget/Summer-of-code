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
    }
}
