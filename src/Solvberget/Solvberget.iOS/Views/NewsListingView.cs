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
	public partial class NewsListingView : MvxTableViewController
    {
		public new NewsListingViewModel ViewModel
		{
			get
			{
				return base.ViewModel as NewsListingViewModel;
			}
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            // Perform any additional setup after loading the view, typically from a nib.
            var source = new MvxStandardTableViewSource(TableView, UITableViewCellStyle.Subtitle, new NSString("TableViewCell"), "TitleText NewsTitle; DetailText Ingress", UITableViewCellAccessory.None);
			TableView.Source = source;

			var loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);

			var set = this.CreateBindingSet<NewsListingView, NewsListingViewModel>();
			set.Bind(source).To(vm => vm.Stories);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);
			Title = ViewModel.Title;
			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			NavigationItem.HidesBackButton = true;

			TableView.ReloadData();
        }
    }
}

