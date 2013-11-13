using System.Collections.Generic;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class EventListViewModel : BaseViewModel
    {
        private List<EventViewModel> _event;
        public List<EventViewModel> Event 
        {
            get { return _event; }
            set { _event = value; RaisePropertyChanged(() => Event);}
        }
    }
}
