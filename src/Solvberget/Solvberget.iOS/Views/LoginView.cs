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

		LoadingOverlay _loadingOverlay = new LoadingOverlay();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Ferdig", UIBarButtonItemStyle.Done, DoLogin), false);

			UsernameLAbel.Font = PinLabel.Font = Application.ThemeColors.LabelFont;
			UsernameLAbel.TextColor = PinLabel.TextColor = Application.ThemeColors.Main;

			Application.ThemeColors.Style(LostCredentialsButton);

			//LostCredentialsButton.TouchUpInside += (s,e) => ViewModel... // todo!!

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
				DoLogin();
				return true;
			};

			Username.BecomeFirstResponder();

        }

		private void DoLogin(object sender = null, EventArgs args = null)
		{
			Password.ResignFirstResponder();
			Add(_loadingOverlay);
			ViewModel.LoginCommand.Execute(null);
		}
    }
}

