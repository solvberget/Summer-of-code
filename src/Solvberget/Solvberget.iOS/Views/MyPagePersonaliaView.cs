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

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UIScrollView container = new UIScrollView(View.Frame);
			container.BackgroundColor = UIColor.White;

			Add(container);
		}
	}
	
}
