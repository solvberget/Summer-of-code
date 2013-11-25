using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class SearchResultCategoryView : MvxFragment
    {
        public SearchResultCategoryView()
        {
            RetainInstance = true;
        }

        public string BindableProperty { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_searchresults, null);

            var resultsList = view.FindViewById<MvxListView>(Resource.Id.searchResultsList);
            var set = this.CreateBindingSet<SearchResultCategoryView, SearchViewModel>();
            set.Bind(resultsList).For(rl => rl.ItemsSource).To(BindableProperty);
            set.Bind(resultsList).For(rl => rl.ItemClick).To(vm => vm.ShowDetailsCommand);
            set.Apply();

            return view;
        }
    }
}