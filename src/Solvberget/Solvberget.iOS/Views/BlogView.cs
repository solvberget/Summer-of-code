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
	public partial class BlogView : NamedTableViewController
    {
		public new BlogViewModel ViewModel
		{
			get
			{
				return base.ViewModel as BlogViewModel;
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

			var source = new SimpleTableViewSource<BlogPostViewModel>(TableView, CellBindings.BlogPosts);
			TableView.Source = source;

			var loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);

			var set = this.CreateBindingSet<BlogView, BlogViewModel>();
			set.Bind(source).To(vm => vm.Posts);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);

			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");

			set.Apply();

			TableView.ReloadData();
        }
    }
}

