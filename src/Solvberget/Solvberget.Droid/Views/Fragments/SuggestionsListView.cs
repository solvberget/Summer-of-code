using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;
using Solvberget.Droid.ActionBar;

namespace Solvberget.Droid.Views.Fragments
{
    public class SuggestionsListView : MvxFragment
    {
        private LoadingIndicator _loadingIndicator;

        private SuggestionsListViewModel _viewModel;
        public new SuggestionsListViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as SuggestionsListViewModel); }
        }

        public SuggestionsListView() 
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreateView(inflater, container, savedInstanceState);
            _loadingIndicator = new LoadingIndicator(Activity);

            var set = this.CreateBindingSet<SuggestionsListView, SuggestionsListViewModel>();
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);
            set.Apply();

            var act = Activity as MvxActionBarActivity;
            if (act != null)
            {
                act.SupportActionBar.Title = ViewModel.Title;
            }

            return this.BindingInflate(Resource.Layout.fragment_suggestions_list, null);
        }
    }
}