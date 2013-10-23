using Cirrious.MvvmCross.ViewModels;

namespace Solvberget.Core.ViewModels.Base
{
    public class BaseViewModel : MvxViewModel
    {
        private long _id;
        /// <summary>
        /// Gets or sets the unique ID for the menu
        /// </summary>
        public long Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(() => this.Id); }
        }

        private string _title = string.Empty;
        /// <summary>
        /// Gets or sets the name of the menu
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        private bool _isLoading;
        public bool IsLoading 
        {
            get { return _isLoading; }
            set { _isLoading = value; RaisePropertyChanged(() => IsLoading);}
        }

    }
}
