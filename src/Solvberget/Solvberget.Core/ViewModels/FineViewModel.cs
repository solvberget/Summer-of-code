using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class FineViewModel : BaseViewModel
    {
        private string _description;
        public string Description
        {
            get { return _description; }
            set{ _description = value; RaisePropertyChanged(() => Description); }
        }

        private string _sum;
        public string Sum
        {
            get { return _sum; }
            set{ _sum = value; RaisePropertyChanged(() => Sum); }
        }

        private string _documentTitle;

        public string DocumentTitle
        {
            get { return _documentTitle; }
            set { _documentTitle = value; RaisePropertyChanged(() => DocumentTitle); }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; RaisePropertyChanged(() => Status); }
        }
    }
}
