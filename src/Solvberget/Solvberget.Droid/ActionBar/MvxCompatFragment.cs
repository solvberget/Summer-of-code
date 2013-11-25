using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.ViewModels;

namespace Solvberget.Droid.ActionBar
{
    public class MvxCompatFragment : MvxEventSourceCompatFragment, IMvxFragmentView
    {
        protected MvxCompatFragment()
        {
            this.AddEventListeners();
        }

        public IMvxBindingContext BindingContext { get; set; }

        private object _dataContext;

        public object DataContext
        {
            get { return _dataContext; }
            set
            {
                _dataContext = value;
                if (BindingContext != null)
                    BindingContext.DataContext = value;
            }
        }

        public virtual IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }
    }
}