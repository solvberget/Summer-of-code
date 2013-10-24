using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class SearchView : MvxFragment
    {
        private LoadingIndicator _loadingIndicator;
        private Android.Widget.SearchView _searchView;

        public SearchView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            SetHasOptionsMenu(true);
            base.OnCreateView(inflater, container, savedInstanceState);
            _loadingIndicator = new LoadingIndicator(Activity);

            var set = this.CreateBindingSet<SearchView, SearchViewModel>();
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);
            set.Apply();

            return this.BindingInflate(Resource.Layout.fragment_search, null);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search_menu, menu);

            _searchView = (Android.Widget.SearchView)menu.FindItem(Resource.Id.search).ActionView;

            _searchView.Iconified = false;
            _searchView.QueryTextSubmit += sView_QueryTextSubmit;
            _searchView.QueryTextChange += sView_QueryTextChange;



            base.OnCreateOptionsMenu(menu, inflater);
        }

        void sView_QueryTextChange(object sender, Android.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            var vm = (SearchViewModel) ViewModel;
            vm.Query = e.NewText;
        }

        void sView_QueryTextSubmit(object sender, Android.Widget.SearchView.QueryTextSubmitEventArgs e)
        {
            var vm = (SearchViewModel)ViewModel;
            vm.SearchAndLoad();

            _searchView.SetQuery("", false);
            _searchView.Iconified = true;

        }
    }
}

