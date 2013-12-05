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
	public partial class SuggestionsListListView : MvxTableViewController
    {
		public new SuggestionsListListViewModel ViewModel
		{
			get
			{
				return base.ViewModel as SuggestionsListListViewModel;
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
			
			var source = new SimpleTableViewSource<SuggestionListSummaryViewModel>(TableView, new SuggestionListSummaryViewModelSimpleTableBinder());
			TableView.Source = source;

			var loadingIndicator = new LoadingOverlay(View.Frame);
			Add(loadingIndicator);

			var set = this.CreateBindingSet<SuggestionsListListView, SuggestionsListListViewModel>();
			set.Bind(source).To(vm => vm.Lists);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowListCommand);
			Title = ViewModel.Title;
			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			NavigationItem.HidesBackButton = true;

			TableView.ReloadData();
        }
    }
}

