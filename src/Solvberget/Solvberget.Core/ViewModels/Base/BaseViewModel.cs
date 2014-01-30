using Cirrious.MvvmCross.ViewModels;
using System.Threading;
using System;

namespace Solvberget.Core.ViewModels.Base
{
    public class BaseViewModel : MvxViewModel
    {
		public static bool AddEmptyItemForEmptyLists = true;

        readonly ManualResetEvent _viewModelReady = new ManualResetEvent(false);

		public void WaitForReady(Action onReady)
		{
			ThreadPool.QueueUserWorkItem(s =>
				{
					_viewModelReady.WaitOne();
					onReady();
				});
		}

		protected void NotifyViewModelReady()
		{
			_viewModelReady.Set();
		}

		public virtual void OnViewReady()
		{
		}

        private long _id;
        /// <summary>
        /// Gets or sets the unique ID for the menu
        /// </summary>
        public long Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(() => Id); }
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
