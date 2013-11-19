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
			
            // Perform any additional setup after loading the view, typically from a nib.
			var source = new MvxStandardTableViewSource(TableView, UITableViewCellStyle.Subtitle, new NSString("TableViewCell"), "TitleText Name; DetailText Title", UITableViewCellAccessory.DisclosureIndicator);
			TableView.Source = source;


			var set = this.CreateBindingSet<SuggestionsListListView, SuggestionsListListViewModel>();
			set.Bind(source).To(vm => vm.Lists);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowListCommand);
			set.Bind().For(v => v.Title).To(vm => vm.Title);
			set.Apply();

			TableView.ReloadData();
        }
    }
}

