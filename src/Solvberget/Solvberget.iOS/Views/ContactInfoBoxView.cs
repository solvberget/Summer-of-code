using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using System.Linq;

namespace Solvberget.iOS
{
	public partial class ContactInfoBoxView : NamedViewController
    {
        public ContactInfoBoxView() : base("ContactInfoBoxView", null)
        {
        }

		public new ContactInfoBoxViewModel ViewModel { get { return base.ViewModel as ContactInfoBoxViewModel; } }

		LoadingOverlay loader = new LoadingOverlay();
		UIScrollView container;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			Add(loader);

			container = new UIScrollView(View.Frame);
			Add(container);

			ViewModel.WaitForReady(() => InvokeOnMainThread(Update));

        }

		private void Update()
		{
			var box = StartBox();

			if(!String.IsNullOrEmpty(ViewModel.Phone)) new LabelAndValue(box, "Telefon", ViewModel.Phone, () => Call(ViewModel.Title, ViewModel.Phone)); // todo: tap to ring
			if(!String.IsNullOrEmpty(ViewModel.Fax)) new LabelAndValue(box, "Faks", ViewModel.Fax);
			if(!String.IsNullOrEmpty(ViewModel.VisitingAddress)) new LabelAndValue(box, "Besøksaddresse", ViewModel.VisitingAddress); // todo: tap to map
			if(!String.IsNullOrEmpty(ViewModel.Email)) new LabelAndValue(box, "Epost", ViewModel.Email, () => Email(ViewModel.Email)); // todo: tap to send

			if (box.Subviews.Length == 0)
			{
				box.RemoveFromSuperview();
				_currentBox = null;
			}

			foreach (var ci in ViewModel.ContactPersons)
			{
				box = StartBox();

				new LabelAndValue(box, null, ci.Position);
				new LabelAndValue(box, "Navn", ci.Name);
				new LabelAndValue(box, "Telefon", ci.Phone, () => Call(ci.Name, ci.Phone));
				new LabelAndValue(box, "Epost", ci.Email, () => Email(ci.Email));
			}

			container.ContentSize = new SizeF(320, container.Subviews.Last().Frame.Bottom + padding);

			loader.Hide();
		}

		UIView _currentBox;

		float padding = 10.0f;

		private UIView StartBox()
		{
			var box = new UIView(new RectangleF(padding, padding, 300,10));
			box.BackgroundColor = Application.ThemeColors.VerySubtle;

			if (null != _currentBox)
			{
				box.Frame = new RectangleF(padding, _currentBox.Frame.Bottom + padding, 300,10);
			}

			_currentBox = box;
			container.Add(box);

			return box;
		}

		private void Call(string name, string number)
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Ringe nå?";
			alert.AddButton("Nei");
			alert.CancelButtonIndex = 0;
			alert.AddButton("Ja");
			alert.Message = "Vil du ringe til " + name + "?";
			alert.Delegate = new PhoneAlertViewDelegate(number);
			alert.Show();
		}

		private void Email(string email)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl("mailto://" + email));
		}
    }

	public class PhoneAlertViewDelegate : UIAlertViewDelegate
	{
		string number;
		public PhoneAlertViewDelegate(string number)
		{
			this.number = number;
		}

		public override void Clicked (UIAlertView alertview, int buttonIndex)
		{
			if(buttonIndex == 1)
			{
				UIApplication.SharedApplication.OpenUrl(new NSUrl("tel:" + number.Replace(" ", String.Empty)));
			}
		}
	}

}

