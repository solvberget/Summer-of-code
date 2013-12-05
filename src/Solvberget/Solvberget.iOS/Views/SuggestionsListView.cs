using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Solvberget.iOS
{
	public partial class SuggestionsListView : MvxTableViewController
    {
		public new SuggestionsListViewModel ViewModel
		{
			get
			{
				return base.ViewModel as SuggestionsListViewModel;
			}
		}

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
			var source = new SimpleTableViewSource<SearchResultViewModel>(TableView, new SearchResultViewModelSimpleTableBinder());
			TableView.Source = source;

			var loadingIndicator = new LoadingOverlay(View.Frame);
			Add(loadingIndicator);

			var set = this.CreateBindingSet<SuggestionsListView, SuggestionsListViewModel>();
			set.Bind(source).To(vm => vm.Docs);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);
			Title = ViewModel.Title;
			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			TableView.ReloadData();
        }
    }
}

