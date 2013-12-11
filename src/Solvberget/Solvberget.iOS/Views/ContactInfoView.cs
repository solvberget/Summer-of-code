using System;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using MonoTouch.UIKit;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.iOS
{
	public class ContactInfoView : NamedTableViewController
	{
		public ContactInfoView() : base()
        {}

		public new ContactInfoViewModel ViewModel { get { return base.ViewModel as ContactInfoViewModel; }}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			LoadingOverlay.LoadingText = "Henter kontaktinformasjon...";
		}

		protected override void ViewModelReady()
		{
			base.ViewModelReady();
		
			var source = new StandardTableViewSource(TableView, UITableViewCellStyle.Default,
				"ContactInfoItems", "TitleText Title");

			TableView.Source = source;

			var set = this.CreateBindingSet<ContactInfoView, ContactInfoViewModel>();
			set.Bind(source).To(vm => vm.InfoBoxes);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);

			set.Apply();

			TableView.ReloadData();
		}
    }
}

