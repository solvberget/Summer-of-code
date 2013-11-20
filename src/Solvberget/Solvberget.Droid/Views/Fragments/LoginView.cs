using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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

        void LoginView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WrongUsernameOrPassword" && ((LoginViewModel)ViewModel).WrongUsernameOrPassword)
            {
                Context context = Application.Context;
                const string text = "Feil brukernavn eller passord";
                const ToastLength duration = ToastLength.Long;

                Toast toast = Toast.MakeText(context, text, duration);
                toast.Show();
            }
            else if (e.PropertyName == "SomethingWentWrong" && ((LoginViewModel)ViewModel).SomethingWentWrong)
            {
                Context context = Application.Context;
                const string text = "Noe gikk galt. Prøv igjen senere";
                const ToastLength duration = ToastLength.Long;

                Toast toast = Toast.MakeText(context, text, duration);
                toast.Show();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            _loadingIndicator = new LoadingIndicator(Activity);

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);
            set.Apply();

            ((LoginViewModel)ViewModel).PropertyChanged += LoginView_PropertyChanged;

            return this.BindingInflate(Resource.Layout.login, null);
        }
    }
}