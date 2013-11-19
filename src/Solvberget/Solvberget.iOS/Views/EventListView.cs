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
	public partial class EventListView : MvxTableViewController
    {
		public new EventListViewModel ViewModel
		{
			get
			{
				return base.ViewModel as EventListViewModel;
			}
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            // Perform any additional setup after loading the view, typically from a nib.
			var source = new MvxStandardTableViewSource(TableView, UITableViewCellStyle.Subtitle, new NSString("TableViewCell"), "TitleText Title; ImageUrl ImageUrl", UITableViewCellAccessory.None);
			TableView.Source = source;


			var set = this.CreateBindingSet<EventListView, EventListViewModel>();
			set.Bind(source).To(vm => vm.Events);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);
            set.Bind().For(v => v.Title).To(vm => vm.Title);
			set.Apply();

			TableView.ReloadData();
        }
    }
}

