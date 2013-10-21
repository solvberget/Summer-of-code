using Android.App;
using Cirrious.MvvmCross.Droid.Views;

namespace Solvberget.Droid
{
    [Activity(
       Label = "Sølvberget"
       , MainLauncher = true
       , Icon = "@drawable/ic_launcher"
       , Theme = "@style/Theme.Splash")]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen() : base(Resource.Layout.SplashScreen)
        {
        }
    }
}
