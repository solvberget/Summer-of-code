using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public partial class ContactInfoBoxView : NamedViewController
    {
        public ContactInfoBoxView() : base("ContactInfoBoxView", null)
        {
        }

		public new ContactInfoBoxViewModel ViewModel { get { return base.ViewModel as ContactInfoBoxViewModel; } }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			ViewModel.WaitForReady(() => InvokeOnMainThread(Update));

        }

		private void Update()
		{

		}
    }
}

