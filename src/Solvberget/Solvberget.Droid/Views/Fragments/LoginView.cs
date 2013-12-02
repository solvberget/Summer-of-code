using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class LoginView : MvxFragment
    {
        private LoadingIndicator _loadingIndicator;

        public LoginView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _loadingIndicator = new LoadingIndicator(Activity);

            ((LoginViewModel)ViewModel).PropertyChanged += LoginView_PropertyChanged;

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);
            set.Apply();

            return this.BindingInflate(Resource.Layout.login, null);
        }

        private void LoginView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var isChanged = e.PropertyName;

            if (isChanged == "ButtonPressed")
            {
                var inputManager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
                inputManager.HideSoftInputFromWindow(View.WindowToken, 0);
            }
        }
    }
}