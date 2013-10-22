using Android.App;
using Android.Support.V4.View;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;

namespace Solvberget.Droid.Views.Fragments
{
    public class SearchView : MvxFragment
    {
        public SearchView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            SetHasOptionsMenu(true);
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_search, null);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search_menu, menu);

            IMenuItem item = menu.FindItem(Resource.Id.search);
            var sView = (Android.Widget.SearchView)item.ActionView;

            sView.Iconified = false;



            base.OnCreateOptionsMenu(menu, inflater);
        }
    }
}