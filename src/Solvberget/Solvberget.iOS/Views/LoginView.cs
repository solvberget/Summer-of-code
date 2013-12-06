using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Solvberget.iOS
{
	public partial class LoginView : NamedViewController
    {
        public LoginView() : base("LoginView", null)
        {
        }

		private new LoginViewModel ViewModel { get { return base.ViewModel as LoginViewModel;}}

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

		LoadingOverlay _loadingOverlay = new LoadingOverlay();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			UsernameLAbel.Font = PinLabel.Font = Application.ThemeColors.LabelFont;
			UsernameLAbel.TextColor = PinLabel.TextColor = Application.ThemeColors.Main;
			LostCredentialsButton.TitleLabel.TextColor = Application.ThemeColors.Main;

			var set = this.CreateBindingSet<LoginView, LoginViewModel>();
			set.Bind(Username).To(vm => vm.UserName);
			set.Bind(Password).To(vm => vm.Pin);
			set.Bind(ErrorMessage).To(vm => vm.Message);

			set.Bind(_loadingOverlay).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");

			set.Apply();

			Username.ShouldReturn += txt =>
			{
				Password.BecomeFirstResponder();
				return true;
			};

			Password.ShouldReturn += txt =>
			{
				Password.ResignFirstResponder();
				Add(_loadingOverlay);
				ViewModel.LoginCommand.Execute(null);
				return true;
			};

			Username.BecomeFirstResponder();

			// lost pin??
        }
    }
}

