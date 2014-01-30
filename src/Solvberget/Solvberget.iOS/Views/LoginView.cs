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

		public new LoginViewModel ViewModel { get { return base.ViewModel as LoginViewModel; }}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Ferdig", UIBarButtonItemStyle.Done, DoLogin), false);

			UsernameLAbel.Font = PinLabel.Font = Application.ThemeColors.LabelFont;
			UsernameLAbel.TextColor = PinLabel.TextColor = Application.ThemeColors.Main;

			LostCredentialsButton.SetTitleColor(Application.ThemeColors.Main, UIControlState.Normal);

			//LostCredentialsButton.TouchUpInside += (s,e) => ViewModel... // todo!!

			var set = this.CreateBindingSet<LoginView, LoginViewModel>();
			set.Bind(Username).To(vm => vm.UserName);
			set.Bind(Password).To(vm => vm.Pin);
			set.Bind(ErrorMessage).To(vm => vm.Message);

			set.Bind(LoadingOverlay).For("Visibility").To(vm => vm.IsLoading).WithConversion("Visibility");

			set.Apply();

			Username.ShouldReturn += txt =>
			{
				Password.BecomeFirstResponder();
				return true;
			};

			Password.ShouldReturn += txt =>
			{
				DoLogin();
				return true;
			};

			Username.BecomeFirstResponder();

        }

		private void DoLogin(object sender = null, EventArgs args = null)
		{
			Password.ResignFirstResponder();
			LoadingOverlay.Show(View);
			ViewModel.LoginCommand.Execute(null);
		}
    }
}

