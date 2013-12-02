using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.ViewModels;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Droid.ActionBar;
using Solvberget.Droid.Helpers;
using Solvberget.Droid.Views.Fragments;
using SearchView = Solvberget.Droid.Views.Fragments.SearchView;

namespace Solvberget.Droid.Views
{
    [Activity(Label = "Sølvberget", LaunchMode = LaunchMode.SingleTop, Theme = "@style/MyTheme", Icon = "@drawable/ic_launcher", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class HomeView : MvxActionBarActivity, IFragmentHost
    {
        private const string START_PAGE_TITLE = "Startside";
        private DrawerLayout _drawer;
        private MyActionBarDrawerToggle _drawerToggle;
        private string _drawerTitle;
        private string _title;
        private MvxListView _drawerList;

        private HomeViewModel _viewModel;
        private MvxFragment _currentFragment;

        public new HomeViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as HomeViewModel); }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SupportFragmentManager.PopBackStackImmediate(null, (int)PopBackStackFlags.Inclusive);
            SetContentView(Resource.Layout.page_home_view);

            _title = _drawerTitle = Title;
            _drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            _drawerList = FindViewById<MvxListView>(Resource.Id.left_drawer);

            
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true); 
            
            

            if (_drawer != null)
            {

                _drawer.SetDrawerShadow(Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);

                //DrawerToggle is the animation that happens with the indicator next to the
                //ActionBar icon. You can choose not to use this.
                _drawerToggle = new MyActionBarDrawerToggle(this, _drawer,
                    Resource.Drawable.ic_drawer_light,
                    Resource.String.drawer_open,
                    Resource.String.drawer_close);

                _drawerToggle.DrawerClosed += delegate
                {
                    
                    Title = _title;
                    SupportInvalidateOptionsMenu();                };

                _drawerToggle.DrawerOpened += delegate
                {
                   
                    SupportActionBar.Title = _drawerTitle;
                    SupportInvalidateOptionsMenu();

                    // Close open soft keyboard when drawer opens.
                    var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
                    inputManager.HideSoftInputFromWindow(Window.DecorView.WindowToken, 0);
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
            customPresenter.Register(typeof (HomeScreenViewModel), this);
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

                var shouldClearBackStack = true;
                var shouldAddToBackStack = false;

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
                    case HomeViewModel.Section.Home:
                    {
                        if (SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as HomeScreenView != null)
                            return true;


                        frag = new HomeScreenView();
                        title = START_PAGE_TITLE;
                        break;
                    }
                    case HomeViewModel.Section.Logout:
                    {
                        //((HomeViewModel)ViewModel).LogOut();
                        Toast.MakeText(Application.Context, "Du er nå logget ut", ToastLength.Long).Show();
                        break;
                    }
                    case HomeViewModel.Section.Unknown:
                    {
                        shouldAddToBackStack = true;
                        if (request.ViewModelType == typeof (SuggestionsListViewModel))
                            frag = new SuggestionsListView();
                        if (request.ViewModelType == typeof(BlogViewModel))
                            frag = new BlogView();
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

                    var trans = SupportFragmentManager.BeginTransaction();
                    if (shouldClearBackStack)
                    {
                        SupportFragmentManager.PopBackStackImmediate(START_PAGE_TITLE, (int) PopBackStackFlags.None);
                    }

                    trans.Replace(Resource.Id.content_frame, frag);
                    _currentFragment = frag;

                    if (shouldAddToBackStack)
                    {
                        trans.AddToBackStack(((BaseViewModel)frag.ViewModel).Title);
                    }
                    trans.Commit();
                }
                

                SupportActionBar.Title = _title = title;
                

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
                //when open don't show anything
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

        public override void OnBackPressed()
        {
            if (SupportFragmentManager.BackStackEntryCount == 0)
            {
                if (_currentFragment is HomeScreenView)
                {
                    base.OnBackPressed();
                }
                else
                {
                    ViewModel.SelectMenuItemCommand.Execute(ViewModel.MenuItems[0]);
                }
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}