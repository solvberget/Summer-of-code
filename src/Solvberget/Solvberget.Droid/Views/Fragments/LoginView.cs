using Android.OS;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;

namespace Solvberget.Droid.Views.Fragments
{
    public class LoginView : MvxFragment
    {
        public LoginView()
        {
            Mvx.Trace("==================LoginView()================");
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Mvx.Trace("==================OnCreateView================");
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.login, null);
        }
    }
}