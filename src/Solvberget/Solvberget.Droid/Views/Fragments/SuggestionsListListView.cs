using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class SuggestionsListListView : MvxFragment
    {
        private LoadingIndicator _loadingIndicator;

        private SuggestionsListListViewModel _viewModel;
        public new SuggestionsListListViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as SuggestionsListListViewModel); }
        }

        public SuggestionsListListView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreateView(inflater, container, savedInstanceState);
            _loadingIndicator = new LoadingIndicator(Activity);

            var set = this.CreateBindingSet<SuggestionsListListView, SuggestionsListListViewModel>();
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);
            set.Apply();

            return this.BindingInflate(Resource.Layout.fragment_suggestions_list_list, null);
        }
    }
}