using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class EventViewModel : BaseViewModel
    {
        private string _uri;
        public string Uri 
        {
            get { return _uri; }
            set { _uri = value; RaisePropertyChanged(() => Uri);}
        }
    }
}