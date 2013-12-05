using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Solvberget.iOS
{
	public class SearchResultViewModelSimpleTableBinder : ISimpleCellBinder<SearchResultViewModel>
	{
		public void Bind(SimpleCell cell, SearchResultViewModel model)
		{
			cell.Bind(model.Name, model.PresentableTypeWithYear, model.Image);
		}
	}

	public static class UIImageHelper
	{
		public static UIImage FromUrl (string uri)
		{
			using (var url = new NSUrl (uri))
			using (var data = NSData.FromUrl(url))
				return null != data ? UIImage.LoadFromData(data) : null;
		}

	}
}

