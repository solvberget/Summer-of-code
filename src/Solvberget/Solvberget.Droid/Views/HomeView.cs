using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Fragging;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.ViewModels;
using Solvberget.Droid.Helpers;
using Solvberget.Droid.Views.Fragments;

namespace Solvberget.Droid.Views
{
    [Activity(Label = "Sølvberget", LaunchMode = LaunchMode.SingleTop, Theme = "@style/MyTheme", Icon = "@drawable/ic_launcher")]
    public class HomeView : MvxFragmentActivity, IFragmentHost
    {
        private DrawerLayout _drawer;
        private MyActionBarDrawerToggle _drawerToggle;
        private string _drawerTitle;
        private string _title;
        private MvxListView _drawerList;

        private HomeViewModel _viewModel;

        public new HomeViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as HomeViewModel); }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.page_home_view);

            _title = _drawerTitle = Title;
            _drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            _drawerList = FindViewById<MvxListView>(Resource.Id.left_drawer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            if (_drawer != null)
            {

                _drawer.SetDrawerShadow(Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);

                //DrawerToggle is the animation that happens with the indicator next to the
                //ActionBar icon. You can choose not to use this.
                _drawerToggle = new MyActionBarDrawerToggle(this, _drawer,
                    Resource.Drawable.ic_drawer_light,
                    Resource.String.drawer_open,
                    Resource.String.drawer_close);

                //You can alternatively use _drawer.DrawerClosed here
                _drawerToggle.DrawerClosed += delegate
                {
                    ActionBar.Title = _title;
                    InvalidateOptionsMenu();
                };


                //You can alternatively use _drawer.DrawerOpened here
                _drawerToggle.DrawerOpened += delegate
                {
                    ActionBar.Title = _drawerTitle;
                    InvalidateOptionsMenu();
                };

                _drawer.SetDrawerListener(_drawerToggle);
            }

            RegisterForDetailsRequests();

            if (null == savedInstanceState)
            {
                ViewModel.SelectMenuItemCommand.Execute(ViewModel.MenuItems[0]);
            }
        }

        /// <summary>
        /// Use the custom presenter to determine if we can navigate forward.
        /// </summary>
        private void RegisterForDetailsRequests()
        {
            var customPresenter = Mvx.Resolve<ICustomPresenter>();
            customPresenter.Register(typeof (MyPageViewModel), this);
            customPresenter.Register(typeof (SearchViewModel), this);
            customPresenter.Register(typeof (NewsListingViewModel), this);
            customPresenter.Register(typeof (OpeningHoursViewModel), this);
            customPresenter.Register(typeof (SuggestionsListListViewModel), this);
            customPresenter.Register(typeof (SuggestionsListViewModel), this);
            customPresenter.Register(typeof (ContactInfoViewModel), this);
            customPresenter.Register(typeof (BlogOverviewViewModel), this);
            customPresenter.Register(typeof (BlogViewModel), this);
            customPresenter.Register(typeof (BlogPostViewModel), this);
            customPresenter.Register(typeof (LoginViewModel), this);
            customPresenter.Register(typeof (EventListViewModel), this);
        }

        /// <summary>
        /// Read all about this, but this is a nice way if there were multiple
        /// fragments on the screen for us to decide what and where to show stuff
        /// See: http://enginecore.blogspot.ro/2013/06/more-dynamic-android-fragments-with.html
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool Show(MvxViewModelRequest request)
        {
            try
            {
                MvxFragment frag = null;
                var title = string.Empty;
                var section = ViewModel.GetSectionForViewModelType(request.ViewModelType);

                switch (section)
                {
                    case HomeViewModel.Section.MyPage:
                    {
                        if (!ViewModel.IsAuthenticated())
                        {
                            if (SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as LoginView != null)
                                return true;

                            frag = new LoginView();
                            title = "Logg inn";
                        }
                        else
                        {
                            if (SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as MyPageView != null)
                                return true;

                            frag = new MyPageView();
                            title = "Min Side";
                            }
                        break;
                    }
                    case HomeViewModel.Section.Search:
                    {
                        if (SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as SearchView != null)
                            return true;

                        frag = new SearchView();
                        title = "Søk";
                        break;
                    }
                    case HomeViewModel.Section.News:
                    {
                        if (SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as NewsListingView != null)
                            return true;

                        frag = new NewsListingView();
                        title = "Nyheter";
                        break;
                    }
                    case HomeViewModel.Section.OpeningHours:
                    {
                        if (SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as OpeningHoursView != null)
                            return true;

                        frag = new OpeningHoursView();
                        title = "Åpningstider";
                        break;
                    }
                    case HomeViewModel.Section.Events:
                    {
                        if (SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as EventListView != null)
                            return true;

                        frag = new EventListView();
                        title = "Arrangement";
                        break;
                    }
                    case HomeViewModel.Section.Lists:
                    {
                        if (SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as SuggestionsListListView != null)
                            return true;

                        frag = new SuggestionsListListView();
                        title = "Anbefalinger";
                        break;
                    }
                    case HomeViewModel.Section.Contact:
                    {
                        if (SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as ContactInfoView != null)
                            return true;
                        
                        frag = new ContactInfoView();
                        title = "Kontaktinformasjon";
                        break;
                    }
                    case HomeViewModel.Section.Blogs:
                    {
                        if (SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as BlogOverviewView != null) 
                            return true;
                        
                        frag = new BlogOverviewView();
                        title = "Blogger";
                        break;
                    }
                    case HomeViewModel.Section.Unknown:
                    {
                        if (request.ViewModelType == typeof (SuggestionsListViewModel))
                            frag = new SuggestionsListView();
                        if (request.ViewModelType == typeof(BlogViewModel))
                            frag = new BlogView();
                        if (request.ViewModelType == typeof (BlogPostViewModel))
                            frag = new BlogPostView();
                        if (request.ViewModelType == typeof(LoginViewModel))
                            frag = new LoginView();
                        break;
                    }
                }

                var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
                var viewModel = loaderService.LoadViewModel(request, null /* saved state */);

                if (frag != null)
                {
                    frag.ViewModel = viewModel;

                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, frag).Commit();
                }
                ActionBar.Title = _title = title;

                ClearAndHighlightActiveMenuItem(section);

                if (_drawer != null)
                {
                    _drawer.CloseDrawer(_drawerList);
                }
                

                return true;
            }
            finally
            {
                if (_drawer != null)
                {
                    _drawer.CloseDrawer(_drawerList);
                }
            }
        }

        private void ClearAndHighlightActiveMenuItem(HomeViewModel.Section section)
        {
            foreach (var menuItem in ViewModel.MenuItems)
            {
                menuItem.IsSelected = false;
            }
            var selectedMenuItem = ViewModel.MenuItems.SingleOrDefault(m => m.Id == (int) section);
            if (selectedMenuItem != null)
                selectedMenuItem.IsSelected = true;
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            if (_drawer != null)
            {
                _drawerToggle.SyncState();
            }
        }


        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (_drawer != null)
            {
                _drawerToggle.OnConfigurationChanged(newConfig);
            }
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            if (_drawer != null)
            {
                var drawerOpen = _drawer.IsDrawerOpen(_drawerList);
                //when open down't show anything
                for (int i = 0; i < menu.Size(); i++)
                    menu.GetItem(i).SetVisible(!drawerOpen);
            }
            
            
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_drawer != null)
            {
                if (_drawerToggle.OnOptionsItemSelected(item))
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}