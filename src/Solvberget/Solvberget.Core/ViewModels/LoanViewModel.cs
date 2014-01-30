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

        private string _dueDate;
        public string DueDate
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

        private string _documentNumber;
        public string DocumentNumber 
        {
            get { return _documentNumber; }
            set { _documentNumber = value; RaisePropertyChanged(() => DocumentNumber);}
        } 

        private MvxCommand<LoanViewModel> _expandLoanCommand;
        public ICommand ExpandLoanCommand
        {
            get
            {
                return _expandLoanCommand ?? (_expandLoanCommand = new MvxCommand<LoanViewModel>(ExecuteExpandLoanCommand));
            }
        }

        private void ExecuteExpandLoanCommand(LoanViewModel loan)
        {
            Parent.ExpandLoan(DocumentNumber);
            
        }
    }
}
