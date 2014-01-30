using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class MyPageFinesView : MvxFragment
    {
        private MyPageFinesViewModel _viewModel;
        public new MyPageFinesViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as MyPageFinesViewModel); }
        }

        public MyPageFinesView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_profile_fines, null);
        }

        public override void OnResume()
        {
            ViewModel.OnViewReady();
            base.OnResume();
        }
    }
}