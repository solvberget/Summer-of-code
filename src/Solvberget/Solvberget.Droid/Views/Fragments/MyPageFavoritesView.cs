using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class MyPageFavoritesView : MvxFragment
    {
        public MyPageFavoritesView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            ((MyPageFavoritesViewModel)ViewModel).PropertyChanged += MyPageFavoritesView_PropertyChanged;

            return this.BindingInflate(Resource.Layout.fragment_profile_favorites, null);
        }

        void MyPageFavoritesView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var isChanged = e.PropertyName;

            if (isChanged == "FavoriteIsRemoved")
            {
                var context = Application.Context;
                const string text = "Favoritt fjernet";
                const ToastLength duration = ToastLength.Long;

                var toast = Toast.MakeText(context, text, duration);
                toast.Show();
            }
        }
    }
}