using System;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class LoanViewModel : BaseViewModel
    {
        private string _documentTitle;
        public string DocumentTitle
        {
            get { return _documentTitle; }
            set
            {
                _documentTitle = value;
                RaisePropertyChanged(() => DocumentTitle);
            }
        }

        private string _subLibrary;
        public string SubLibrary
        {
            get { return _subLibrary; }
            set
            {
                _subLibrary = value;
                RaisePropertyChanged(() => SubLibrary);
            }
        }

        private DateTime? _dueDate;
        public DateTime? DueDate
        {
            get { return _dueDate; }
            set
            {
                _dueDate = value;
                RaisePropertyChanged(() => DueDate);
            }
        }

        private string _material;
        public string Material
        {
            get { return _material; }
            set
            {
                _material = value;
                RaisePropertyChanged(() => Material);
            }
        }
    }
}
