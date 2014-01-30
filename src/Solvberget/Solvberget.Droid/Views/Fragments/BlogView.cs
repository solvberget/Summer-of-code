using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;
using Solvberget.Droid.ActionBar;

namespace Solvberget.Droid.Views.Fragments
{
    public class BlogView : MvxFragment
    {
        private LoadingIndicator _loadingIndicator;

        private BlogViewModel _viewModel;
        public new BlogViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as BlogViewModel); }
        }

        public BlogView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreateView(inflater, container, savedInstanceState);
            _loadingIndicator = new LoadingIndicator(Activity);

            var set = this.CreateBindingSet<BlogView, BlogViewModel>();
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);

            set.Apply();

            var act = Activity as MvxActionBarActivity;
            if (act != null)
            {
                act.SupportActionBar.Title = ViewModel.Title;
            }

            return this.BindingInflate(Resource.Layout.fragment_blog, null);
        }

        public override void OnResume()
        {
            ViewModel.OnViewReady();
            base.OnResume();
        }
    }
}