using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class BlogOverviewView : MvxFragment
    {
        private LoadingIndicator _loadingIndicator;

        public BlogOverviewView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            SetHasOptionsMenu(true);
            base.OnCreateView(inflater, container, savedInstanceState);
            _loadingIndicator = new LoadingIndicator(Activity);

            return this.BindingInflate(Resource.Layout.fragment_blogoverview, null);
        }
    }
}