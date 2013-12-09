using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Solvberget.iOS
{
	public class MyPagePersonaliaView : NamedViewController
	{
		public new MyPagePersonaliaViewModel ViewModel
		{
			get
			{
				return base.ViewModel as MyPagePersonaliaViewModel;
			}
		}

		LoadingOverlay loadingIndicator = new LoadingOverlay();

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Add(loadingIndicator);

			ViewModel.WaitForReady(() => InvokeOnMainThread(Update));
		}

		private void Update()
		{
			UIScrollView scrollView = new UIScrollView(new RectangleF(PointF.Empty,View.Frame.Size));
			scrollView.BackgroundColor = UIColor.White;

			Add(scrollView);

			var boxes = new BoxRenderer(scrollView);

			var box = boxes.StartBox();

			new LabelAndValue(box, "Navn", ViewModel.Name);
			new LabelAndValue(box, "Fødselsdato", ViewModel.DateOfBirth);

			box = boxes.StartBox();
			new LabelAndValue(box, "Addresse", ViewModel.StreetAdress + Environment.NewLine + ViewModel.CityAdress);
			new LabelAndValue(box, "Epost", ViewModel.Email);
			new LabelAndValue(box, "Mobiltelefon", ViewModel.CellPhoneNumber);

			box = boxes.StartBox();
			new LabelAndValue(box, "Bibliotek", ViewModel.HomeLibrary);
			new LabelAndValue(box, "Balanse", ViewModel.Balance);
			new LabelAndValue(box, "Kredittgrense", ViewModel.Credit);

			loadingIndicator.Hide();

			UIHelpers.SetContentSize(scrollView);
		}
	}
	
}
