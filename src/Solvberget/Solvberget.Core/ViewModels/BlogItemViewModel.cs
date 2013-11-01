using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class BlogItemViewModel : BaseViewModel
    {
        private string _description;
        public string Description 
        {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(() => Description);}
        }
    }
}