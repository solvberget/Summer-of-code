// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace Solvberget.iOS
{
	[Register ("LoginView")]
	partial class LoginView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel ErrorMessage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton LoginButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton LostCredentialsButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField Password { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel PinLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField Username { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel UsernameLAbel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (LoginButton != null) {
				LoginButton.Dispose ();
				LoginButton = null;
			}

			if (LostCredentialsButton != null) {
				LostCredentialsButton.Dispose ();
				LostCredentialsButton = null;
			}

			if (Password != null) {
				Password.Dispose ();
				Password = null;
			}

			if (PinLabel != null) {
				PinLabel.Dispose ();
				PinLabel = null;
			}

			if (Username != null) {
				Username.Dispose ();
				Username = null;
			}

			if (UsernameLAbel != null) {
				UsernameLAbel.Dispose ();
				UsernameLAbel = null;
			}

			if (ErrorMessage != null) {
				ErrorMessage.Dispose ();
				ErrorMessage = null;
			}
		}
	}
}
