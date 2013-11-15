using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Views;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    [Activity(Label = "Mediadetaljer", Theme = "@style/MyTheme", Icon = "@android:color/transparent", ParentActivity = typeof(HomeView))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "solvberget.droid.views.HomeView")]
    public class MediaDetailView : MvxActivity
    {
        private LoadingIndicator _loadingIndicator;
        private ShareActionProvider _shareActionProvider;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.fragment_mediadetail);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            BindLoadingIndicator();
        }

        private void BindLoadingIndicator()
        {
            _loadingIndicator = new LoadingIndicator(this);

            var set = this.CreateBindingSet<MediaDetailView, SearchResultViewModel>();
            set.Bind(ActionBar).For(v => v.Title).To(vm => vm.Title).Mode(MvxBindingMode.OneWay);
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);
            set.Apply();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    NavUtils.NavigateUpFromSameTask(this);
                    break;
                case Resource.Menu.star:
                    ((MediaDetailViewModel) ViewModel).AddFavorite();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.star, menu);
            MenuInflater.Inflate(Resource.Menu.share, menu);
            // Locate MenuItem with ShareActionProvider
            IMenuItem shareMenuItem = menu.FindItem(Resource.Id.menu_share);
            _shareActionProvider = (ShareActionProvider) shareMenuItem.ActionProvider;

            CreateShareMenu();

            return base.OnCreateOptionsMenu(menu);
        }

        private void CreateShareMenu()
        {
            if (_shareActionProvider != null)
            {
                var playStoreLink = "https://play.google.com/store/apps/details?id=" + PackageName;
                var shareTextBody = string.Format(
                    "Jeg søkte og fant {0} hos Sølvberget. Last ned appen på Google Play for muligheten til å låne den du også. {1}",
                    ((MediaDetailViewModel) ViewModel).Title,
                    playStoreLink);

                var shareIntent = ShareCompat.IntentBuilder.From(this)
                    .SetType("text/plain")
                    .SetText(shareTextBody)
                    .SetSubject("Sølvberget")
                    .Intent;
                _shareActionProvider.SetShareIntent(shareIntent);
            }
        }
    }
}