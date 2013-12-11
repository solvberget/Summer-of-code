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

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			LoadingOverlay.LoadingText = "Henter kontaktinformasjon...";
		}

		protected override void ViewModelReady()
		{
			base.ViewModelReady();
		
			foreach (var s in ScrollView.Subviews) s.RemoveFromSuperview();

			var boxes = new BoxRenderer(ScrollView);
			var box = boxes.StartBox();

			if(!String.IsNullOrEmpty(ViewModel.Phone)) new LabelAndValue(box, "Telefon", ViewModel.Phone, onTap : () => Call(ViewModel.Title, ViewModel.Phone)); // todo: tap to ring
			if(!String.IsNullOrEmpty(ViewModel.Fax)) new LabelAndValue(box, "Faks", ViewModel.Fax);
			if(!String.IsNullOrEmpty(ViewModel.VisitingAddress)) new LabelAndValue(box, "Besøksaddresse", ViewModel.VisitingAddress); // todo: tap to map
			if(!String.IsNullOrEmpty(ViewModel.Email)) new LabelAndValue(box, "Epost", ViewModel.Email, onTap : () => Email(ViewModel.Email)); // todo: tap to send

			if (box.Subviews.Length == 0)
			{
				box.RemoveFromSuperview();
			}

			foreach (var ci in ViewModel.ContactPersons)
			{
				box = boxes.StartBox();

				new LabelAndValue(box, null, ci.Position, true);
				new LabelAndValue(box, "Navn", ci.Name);
				new LabelAndValue(box, "Telefon", ci.Phone, onTap : () => Call(ci.Name, ci.Phone));
				new LabelAndValue(box, "Epost", ci.Email, onTap : () => Email(ci.Email));
			}

			UIHelpers.SetContentSize(ScrollView);
		}

		private void Call(string name, string number)
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Ringe nå?";
			alert.AddButton("Nei");
			alert.CancelButtonIndex = 0;
			alert.AddButton("Ja");
			alert.Message = "Vil du ringe til " + name + "?";

			alert.Dismissed += (sender, e) =>
			{
				if (e.ButtonIndex == 1)
				{
					number = number
						.Replace("(", String.Empty)
						.Replace(")", String.Empty)
						.Replace(" ", String.Empty);

					UIApplication.SharedApplication.OpenUrl(new NSUrl("tel:" + number.Replace(" ", String.Empty)));
				}
			};

			alert.Show();
		}

		private void Email(string email)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl("mailto://" + email));
		}
    }

}

