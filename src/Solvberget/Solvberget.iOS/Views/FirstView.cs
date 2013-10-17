using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS.Views
{
    [Register("FirstView")]
    public class FirstView : MvxViewController
    {
        public override void ViewDidLoad()
        {
            View = new UIView();

            base.ViewDidLoad();
            var uiLabel = new UILabel(new RectangleF(0, 0, 320, 100));
            View.AddSubview(uiLabel);

            this.CreateBinding(uiLabel).To<FirstViewModel>(vm => vm.Hello).Apply();

            // Perform any additional setup after loading the view
        }
    }
}