using System.Collections.Generic;
using Android.Support.V4.View;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;
using Solvberget.Droid.Views.Adapters;

namespace Solvberget.Droid.Views.Fragments
{
    public class MyPageView : MvxFragment
    {
        private MyPageViewModel _viewModel;
        public new MyPageViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as MyPageViewModel); }
        }

        private ViewPager _viewPager;
        private MvxViewPagerFragmentAdapter _adapter;

        public MyPageView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            SetHasOptionsMenu(true);
            base.OnCreateView(inflater, container, savedInstanceState);

            View view;

            //if (ViewModel.LoggedIn)
            //{
                view = this.BindingInflate(Resource.Layout.fragment_profile, null);

                _viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager);
                _viewPager.OffscreenPageLimit = 4;

                var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo>
              {
                  new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                  FragmentType = typeof(MyPagePersonaliaView),
                  Title = "Personalia",
                  ViewModel = ViewModel.MyPagePersonaliaViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                  FragmentType = typeof(MyPageMessagesView),
                  Title = "Meldinger",
                  ViewModel = ViewModel.MyPageMessagesViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                  FragmentType = typeof(MyPageFavoritesView),
                  Title = "Favoritter",
                  ViewModel = ViewModel.MyPageFavoritesViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                  FragmentType = typeof(MyPageLoansView),
                  Title = "Lån",
                  ViewModel = ViewModel.MyPageLoansViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                  FragmentType = typeof(MyPageReservationsView),
                  Title = "Reservasjoner",
                  ViewModel = ViewModel.MyPageReservationsViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                  FragmentType = typeof(MyPageFinesView),
                  Title = "Gebyrer",
                  ViewModel = ViewModel.MyPageFinesViewModel
                }
              };

                _adapter = new MvxViewPagerFragmentAdapter(Activity, ChildFragmentManager, fragments);
                _viewPager.Adapter = _adapter;   
            //}
            //else
            //{
            //    view = this.BindingInflate(Resource.Layout.login, null);
            //}
            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.refresh, menu);
        }
    }
}

