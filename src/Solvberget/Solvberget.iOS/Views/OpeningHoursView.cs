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
	public partial class OpeningHoursView : MvxTableViewController
    {
		public new OpeningHoursViewModel ViewModel
		{
			get
			{
				return base.ViewModel as OpeningHoursViewModel;
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
			
            // Perform any additional setup after loading the view, typically from a nib.
			var source = new MvxSimpleTableViewSource(TableView, OpeningHoursLocationCell.Key, OpeningHoursLocationCell.Key);
			TableView.Source = source;


			var loadingIndicator = new LoadingOverlay(View.Frame);
			Add(loadingIndicator);

			var set = this.CreateBindingSet<OpeningHoursView, OpeningHoursViewModel>();
			set.Bind(source).To(vm => vm.Locations);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);
			set.Bind().For(v => v.Title).To(vm => vm.Title);
			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			NavigationItem.HidesBackButton = true;

			TableView.ReloadData();
        }
    }
}

