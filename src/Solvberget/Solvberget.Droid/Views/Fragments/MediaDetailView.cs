using Android.App;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;
using Solvberget.Droid.ActionBar;
using ShareActionProvider = Android.Support.V7.Widget.ShareActionProvider;

namespace Solvberget.Droid.Views.Fragments
{
    public class MediaDetailView : MvxFragment
    {
        private readonly HomeViewModel _homeVm;
        private LoadingIndicator _loadingIndicator;
        private ShareActionProvider _shareActionProvider;
        private IMenu _menu;
        private MenuInflater _inflater;


        private MediaDetailViewModel _viewModel;
        public new MediaDetailViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as MediaDetailViewModel); }
        }

        public MediaDetailView()
        {
            RetainInstance = true;
        }

        public MediaDetailView(HomeViewModel homeVm)
        {
            RetainInstance = true;
            _homeVm = homeVm;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_mediadetail, null);

            _loadingIndicator = new LoadingIndicator(Activity);

            var set = this.CreateBindingSet<MediaDetailView, MediaDetailViewModel>();
            set.Bind(_loadingIndicator).For(loadingIndicator => loadingIndicator.Visible).To(vm => vm.IsLoading);
            set.Apply();

            ViewModel.PropertyChanged += MediaDetailView_PropertyChanged;

            var act = Activity as MvxActionBarActivity;
            if (act != null)
            {
                act.SupportActionBar.Title = ViewModel.Title;
                ViewModel.WaitForReady(act.InvalidateOptionsMenu);
            }

            return view;
        }

        void MediaDetailView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsFavorite":
                    LoadMenu();
                    if (ViewModel.IsFavorite)
                        Toast.MakeText(Application.Context, "Lagt til som favoritt", ToastLength.Long).Show();
                    break;
                case "IsReservable":
                    if (ViewModel.IsReservedByUser)
                        Toast.MakeText(Application.Context, "Dokumentet er reservert", ToastLength.Long).Show();
                    break;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_is_not_favorite:
                    ViewModel.AddFavorite();
                    break;
                case Resource.Id.menu_is_favorite:
                    ViewModel.RemoveFavorite();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            _menu = menu;
            _inflater = inflater;

            LoadMenu();
        }

        private void LoadMenu()
        {
            _menu.Clear();
            _inflater.Inflate(
                ViewModel.IsFavorite
                    ? Resource.Menu.star_is_favorite
                    : Resource.Menu.star_is_not_favorite, _menu);

            _inflater.Inflate(Resource.Menu.share, _menu);
            // Locate MenuItem with ShareActionProvider
            var inflatedShareView = _menu.FindItem(Resource.Id.menu_share);
            var actionShareView = new ShareActionProvider(Activity);
            MenuItemCompat.SetActionProvider(inflatedShareView, actionShareView);

            _shareActionProvider = actionShareView;

            CreateShareMenu();

            base.OnCreateOptionsMenu(_menu, _inflater);
        }

        private void CreateShareMenu()
        {
            if (_shareActionProvider == null) return;

            var shareIntent = ShareCompat.IntentBuilder.From(Activity)
                                         .SetType("text/plain")
                                         .SetText(ViewModel.WebAppUrl)
                                         .SetSubject(ViewModel.Title)
                                         .Intent;
            _shareActionProvider.SetShareIntent(shareIntent);
        }

        public override void OnResume()
        {
            ViewModel.OnViewReady();
            base.OnResume();
        }
    }
}