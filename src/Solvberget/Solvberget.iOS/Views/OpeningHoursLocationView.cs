using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Solvberget.iOS
{
	public partial class OpeningHoursLocationView : NamedTableViewController
    {
		public new OpeningHoursLocationViewModel ViewModel { get { return base.ViewModel as OpeningHoursLocationViewModel; } }

		LoadingOverlay loadingIndicator;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			loadingIndicator = new LoadingOverlay();
			Add(loadingIndicator);

			ViewModel.WaitForReady(() => InvokeOnMainThread(Update));
        }

		private void Update()
		{
			var source = new StandardTableViewSource(TableView, 
				"TitleText Key + If(Value.Length>0, ': ','') + Value");


			TableView.Source = source;
			TableView.AllowsSelection = false;

			var set = this.CreateBindingSet<OpeningHoursLocationView, OpeningHoursLocationViewModel>();
			set.Bind(source).To(vm => vm.Hours);

			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();

			TableView.ReloadData();

			loadingIndicator.Hide();
		}
    }
}

