using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class LoginView : MvxFragment
    {
        private readonly HomeViewModel _homeVm;
        private LoadingIndicator _loadingIndicator;
        
        private LoginViewModel _viewModel;
        public new LoginViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as LoginViewModel); }
        }

        public LoginView(HomeViewModel homeVm)
        {
            _homeVm = homeVm;
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _loadingIndicator = new LoadingIndicator(Activity);

            ViewModel.PropertyChanged += LoginView_PropertyChanged;

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);
            set.Apply();

            var view = this.BindingInflate(Resource.Layout.login, null);

            var lostPassButton = view.FindViewById(Resource.Id.buttonLostPass);
            lostPassButton.Click += lostPassButton_Click;


            return view;
        }

        void lostPassButton_Click(object sender, System.EventArgs e)
        {
            var builder = new AlertDialog.Builder(Activity);
            // Get the layout inflater
            var inflater = Activity.LayoutInflater;

            
            builder.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
            builder.SetTitle("Glemt PIN");
            builder.SetView(inflater.Inflate(Resource.Layout.dialog_forgotpass, null));  
            builder.SetPositiveButton("Send PIN", (source, args) =>
                {
                    var view = ((Dialog) source).FindViewById(Resource.Id.forgotPassUsername) as TextView;
                    if (view == null) { return; }

                    ViewModel.ForgotPasswordCommand.Execute(view.Text);
                });
            builder.SetNegativeButton("Avbryt", (source, args) => { });
            builder.Create().Show();
        }

        private void LoginView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var isChanged = e.PropertyName;

            if (isChanged == "ButtonPressed")
            {
                var inputManager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
                inputManager.HideSoftInputFromWindow(View.WindowToken, 0);
            }

            if (isChanged == "LoggedIn")
            {
                _homeVm.LoggedIn = ViewModel.LoggedIn;
            }
        }
    }
}