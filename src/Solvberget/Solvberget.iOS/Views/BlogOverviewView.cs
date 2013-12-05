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
	public partial class BlogOverviewView : NamedTableViewController
    {
		public new BlogOverviewViewModel ViewModel
		{
			get
			{
				return base.ViewModel as BlogOverviewViewModel;
			}
		}

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			var source = new SimpleTableViewSource<BlogItemViewModel>(TableView, new BlogSimpleCellBinder());

			TableView.Source = source;

			var loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);
				
			var set = this.CreateBindingSet<BlogOverviewView, BlogOverviewViewModel>();
			set.Bind(source).To(vm => vm.Blogs);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);
			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");

			set.Apply();

			TableView.ReloadData();
        }
    }
}

