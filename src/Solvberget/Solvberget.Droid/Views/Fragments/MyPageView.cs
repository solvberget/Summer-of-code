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
        private MyPageViewModel viewModel;

        public new MyPageViewModel ViewModel
        {
            get { return viewModel ?? (viewModel = base.ViewModel as MyPageViewModel); }
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
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_profile, null);

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
                  FragmentType = typeof(MyPageLoansView),
                  Title = "Mine lån",
                  ViewModel = ViewModel.MyPageLoansViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                  FragmentType = typeof(MyPageReservationsView),
                  Title = "Mine reservasjoner",
                  ViewModel = ViewModel.MyPageReservationsViewModel
                },
              };

            _adapter = new MvxViewPagerFragmentAdapter(Activity, ChildFragmentManager, fragments);
            _viewPager.Adapter = _adapter;

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.refresh, menu);
        }
    }
}