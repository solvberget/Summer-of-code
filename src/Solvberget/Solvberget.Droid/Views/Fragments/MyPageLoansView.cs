using System.ComponentModel;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class MyPageLoansView : MvxFragment
    {
        private MyPageLoansViewModel _viewModel;
        public new MyPageLoansViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as MyPageLoansViewModel); }
        }

        public MyPageLoansView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            ViewModel.PropertyChanged += MyPageLoansView_PropertyChanged;

            return this.BindingInflate(Resource.Layout.fragment_profile_loans, null);
        }

        private void MyPageLoansView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var isChanged = e.PropertyName;

            if (isChanged == "RenewalStatus")
                Toast.MakeText(Application.Context, ViewModel.RenewalStatus, ToastLength.Long).Show();
        }
        public override void OnResume()
        {
            ViewModel.OnViewReady();
            base.OnResume();
        }
    }
}