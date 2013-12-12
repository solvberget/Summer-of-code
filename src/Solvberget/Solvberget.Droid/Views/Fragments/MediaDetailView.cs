using Android.App;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;
using ShareActionProvider = Android.Support.V7.Widget.ShareActionProvider;

namespace Solvberget.Droid.Views.Fragments
{
    public class MediaDetailView : MvxFragment
    {
        private LoadingIndicator _loadingIndicator;
        private ShareActionProvider _shareActionProvider;
        private IMenu _menu;
        private bool _starIsClicked;
        private MenuInflater _inflater;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_mediadetail, null);

            _loadingIndicator = new LoadingIndicator(Activity);

            var set = this.CreateBindingSet<MediaDetailView, MediaDetailViewModel>();
            set.Bind(_loadingIndicator).For(loadingIndicator => loadingIndicator.Visible).To(vm => vm.IsLoading);
            set.Apply();

            ((MediaDetailViewModel)ViewModel).PropertyChanged += MediaDetailView_PropertyChanged;

            return view;
        }

        void MediaDetailView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsFavorite":
                    LoadMenu();
                    if (((MediaDetailViewModel)ViewModel).IsFavorite)
                        Toast.MakeText(Application.Context, "Lagt til som favoritt", ToastLength.Long).Show();
                    break;
                case "IsReservable":
                    if (((MediaDetailViewModel)ViewModel).IsReservedByUser)
                        Toast.MakeText(Application.Context, "Dokumentet er reservert", ToastLength.Long).Show();
                    break;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    NavUtils.NavigateUpFromSameTask(Activity);
                    break;
                case Resource.Id.menu_is_not_favorite:
                    if (((MediaDetailViewModel)ViewModel).LoggedIn)
                    {
                        _starIsClicked = true;
                        ((MediaDetailViewModel)ViewModel).AddFavorite();
                    }
                    else
                    {
                        ShowLoginDialog();
                    }

                    break;
                case Resource.Id.menu_is_favorite:
                    _starIsClicked = true;
                    ((MediaDetailViewModel)ViewModel).RemoveFavorite();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void ShowLoginDialog()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            // Get the layout inflater
            LayoutInflater inflater = Activity.LayoutInflater;


            builder.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
            builder.SetTitle("Logg inn");
            builder.SetView(inflater.Inflate(Resource.Layout.dialog_login, null));
            builder.SetPositiveButton("Logg inn", async (source, args) =>
                {
                    var vm = (MediaDetailViewModel) ViewModel;
                    var username = ((EditText)((Dialog) source).FindViewById(Resource.Id.dialogLoginUsername)).Text;
                    var password =  ((EditText)((Dialog) source).FindViewById(Resource.Id.dialogLoginPin)).Text;
                    var success = await vm.Login(username, password);
                    if (success)
                    {
                        _starIsClicked = true;
                        ((MediaDetailViewModel)ViewModel).AddFavoriteNoRedirect();
                    }
                });
            builder.SetNegativeButton("Avbryt", (source, args) => { });
            builder.Create().Show();
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
                ((MediaDetailViewModel)ViewModel).IsFavorite
                    ? Resource.Menu.star_is_favorite
                    : Resource.Menu.star_is_not_favorite, _menu);

            _inflater.Inflate(Resource.Menu.share, _menu);
            // Locate MenuItem with ShareActionProvider
            var inflatedShareView = _menu.FindItem(Resource.Id.menu_share);
            var actionShareView = new ShareActionProvider(Activity);
            MenuItemCompat.SetActionProvider(inflatedShareView, actionShareView);

            _shareActionProvider = actionShareView;

            CreateShareMenu();

            _starIsClicked = false;

            base.OnCreateOptionsMenu(_menu, _inflater);
        }

        private void CreateShareMenu()
        {
            if (_shareActionProvider != null)
            {
                var playStoreLink = "https://play.google.com/store/apps/details?id=" + Activity.PackageName;
                var shareTextBody = string.Format(
                    "Jeg fant {0} hos Sølvberget. Last ned app for muligheten til å låne du også: {1}",
                    ((MediaDetailViewModel)ViewModel).Title,
                    playStoreLink);

                var shareIntent = ShareCompat.IntentBuilder.From(Activity)
                    .SetType("text/plain")
                    .SetText(shareTextBody)
                    .SetSubject("Sølvberget")
                    .Intent;
                _shareActionProvider.SetShareIntent(shareIntent);
            }
        }

        public override void OnResume()
        {
            if (ViewModel != null)
            {
                var vm = (MediaDetailViewModel)ViewModel;
                vm.OnViewReady();
            }

            base.OnResume();
        }
    }
}