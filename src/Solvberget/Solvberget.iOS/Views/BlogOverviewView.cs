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

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			LoadingOverlay.LoadingText = "Henter blogger...";
		}
	
		protected override void ViewModelReady()
		{
			base.ViewModelReady();

			var source = new SimpleTableViewSource<BlogItemViewModel>(TableView, CellBindings.Blogs);

			TableView.Source = source;

			var set = this.CreateBindingSet<BlogOverviewView, BlogOverviewViewModel>();
			set.Bind(source).To(vm => vm.Blogs);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);

			set.Apply();

		}
    }
}

