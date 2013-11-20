using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public partial class MediaDetailView : MvxViewController
    {
		public new MediaDetailViewModel ViewModel
		{
			get
			{
				return base.ViewModel as MediaDetailViewModel;
			}
		}


        public MediaDetailView() : base("MediaDetailView", null)
        {
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
			HeaderView.BackgroundColor = UIColor.Red;

			var loadingIndicator = new LoadingOverlay(View.Frame);
			Add(loadingIndicator);

			var set = this.CreateBindingSet<MediaDetailView, MediaDetailViewModel>();
			set.Bind().For(v => v.Title).To(vm => vm.Title);
			set.Bind().For(v => v.Image).To(vm => vm.Image);
			set.Bind().For(v => v.MediaTitle).To(vm => vm.Title);
			set.Bind().For(v => v.MediaSubtitle).To(vm => vm.SubTitle);
			set.Bind(loadingIndicator).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");
			set.Apply();
        }
    }
}

