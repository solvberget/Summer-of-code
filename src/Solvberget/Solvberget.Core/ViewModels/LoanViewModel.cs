using System;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
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

        private MyPageLoansViewModel _parent;
        public MyPageLoansViewModel Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                RaisePropertyChanged(() => Parent);
            }
        }

        private bool _buttonVisible;
        public bool ButtonVisible
        {
            get { return _buttonVisible; }
            set
            {
                _buttonVisible = value;
                RaisePropertyChanged(() => ButtonVisible);
            }
        }

        private string _image;
        public string Image 
        {
            get { return _image; }
            set { _image = value; RaisePropertyChanged(() => Image);}
        }

        private MvxCommand<LoanViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<LoanViewModel>(ExecuteShowDetailsCommand));
            }
        }

        private void ExecuteShowDetailsCommand(LoanViewModel loan)
        {

            //Send request to expand due date, give feedback on result.
            
        }
    }
}
