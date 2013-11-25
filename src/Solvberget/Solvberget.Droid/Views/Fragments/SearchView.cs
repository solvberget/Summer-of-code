using System;
using System.Collections.Generic;
using Android.Support.V4.View;
using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Solvberget.Core.ViewModels;
using Solvberget.Droid.ActionBar;
using Solvberget.Droid.Views.Adapters;

namespace Solvberget.Droid.Views.Fragments
{
    public class SearchView : MvxCompatFragment
    {
        private LoadingIndicator _loadingIndicator;
        private Android.Support.V7.Widget.SearchView _searchView;
        private ViewPager _viewPager;
        private MvxViewPagerSearchResultFragmentAdapter _adapter;

        public SearchView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_search, null);

            _loadingIndicator = new LoadingIndicator(Activity);

            var set = this.CreateBindingSet<SearchView, SearchViewModel>();
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);
            set.Apply();

            _viewPager = view.FindViewById<ViewPager>(Resource.Id.searchViewPager);
            _viewPager.OffscreenPageLimit = 4;

            var fragments = new List<MvxViewPagerSearchResultFragmentAdapter.SearchResultFragmentInfo>
              {
                  new MvxViewPagerSearchResultFragmentAdapter.SearchResultFragmentInfo
                  {
                      Title = "Alle",
                      ViewModel = ViewModel,
                      BindableProperty = "Results"
                  },
                  new MvxViewPagerSearchResultFragmentAdapter.SearchResultFragmentInfo
                  {
                      Title = "Bøker",
                      ViewModel = ViewModel,
                      BindableProperty = "BookResults"
                  },
                  new MvxViewPagerSearchResultFragmentAdapter.SearchResultFragmentInfo
                  {
                      Title = "Filmer",
                      ViewModel = ViewModel,
                      BindableProperty = "MovieResults"
                  },
                  new MvxViewPagerSearchResultFragmentAdapter.SearchResultFragmentInfo
                  {
                      Title = "Lydbøker",
                      ViewModel = ViewModel,
                      BindableProperty = "AudioBookResults"
                  },
                  new MvxViewPagerSearchResultFragmentAdapter.SearchResultFragmentInfo
                  {
                      Title = "CDer",
                      ViewModel = ViewModel,
                      BindableProperty = "CDResults"
                  },
                  new MvxViewPagerSearchResultFragmentAdapter.SearchResultFragmentInfo
                  {
                      Title = "Tidsskrift",
                      ViewModel = ViewModel,
                      BindableProperty = "MagazineResults"
                  },
                  new MvxViewPagerSearchResultFragmentAdapter.SearchResultFragmentInfo
                  {
                      Title = "Noter",
                      ViewModel = ViewModel,
                      BindableProperty = "SheetMusicResults"
                  },
                  new MvxViewPagerSearchResultFragmentAdapter.SearchResultFragmentInfo
                  {
                      Title = "Spill",
                      ViewModel = ViewModel,
                      BindableProperty = "GameResults"
                  },
                  new MvxViewPagerSearchResultFragmentAdapter.SearchResultFragmentInfo
                  {
                      Title = "Annet",
                      ViewModel = ViewModel,
                      BindableProperty = "OtherResults"
                  },
              };

            _adapter = new MvxViewPagerSearchResultFragmentAdapter(Activity, ChildFragmentManager, fragments);
            _viewPager.Adapter = _adapter;

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search_menu, menu);

            var inflatedSearchView = menu.FindItem(Resource.Id.search);
            var actionSearchView = new Android.Support.V7.Widget.SearchView(Activity);
            inflatedSearchView.SetActionView(actionSearchView);

            var actionView = MenuItemCompat.GetActionView(inflatedSearchView);

            try
            {
                _searchView = (Android.Support.V7.Widget.SearchView)actionView;
            }
            catch (Exception e)
            {
                
            }

            if (_searchView != null)
            {
                _searchView.Iconified = false;
                _searchView.QueryTextSubmit += sView_QueryTextSubmit;
                _searchView.QueryTextChange += sView_QueryTextChange;
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        void sView_QueryTextChange(object sender, Android.Support.V7.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            var vm = (SearchViewModel) ViewModel;
            vm.Query = e.NewText;
        }

        void sView_QueryTextSubmit(object sender, Android.Support.V7.Widget.SearchView.QueryTextSubmitEventArgs e)
        {
            var vm = (SearchViewModel)ViewModel;
            vm.SearchAndLoad();

            _searchView.SetQuery("", false);
            _searchView.Iconified = true;
        }
    }
}

