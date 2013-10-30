using System.Collections.Generic;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class ContactInfoViewModel : BaseViewModel
    {
        private List<ContactInfoBoxViewModel> _infoBoxes;
        public List<ContactInfoBoxViewModel> InfoBoxes 
        {
            get { return _infoBoxes; }
            set { _infoBoxes = value; RaisePropertyChanged(() => InfoBoxes);}
        }
    }
}
