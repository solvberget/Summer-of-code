using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS.Views
{
    [Register("HomeView")]
    public class HomeView : MvxViewController
    {
        public override void ViewDidLoad()
        {
            View = new UIView();

            base.ViewDidLoad();
            var uiLabel = new UILabel(new RectangleF(0, 0, 320, 100));
            View.AddSubview(uiLabel);

			uiLabel.Text = "Hello World";
            // Perform any additional setup after loading the view
        }
    }
}