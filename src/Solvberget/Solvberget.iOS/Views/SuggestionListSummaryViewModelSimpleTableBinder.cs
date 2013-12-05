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
	public class SuggestionListSummaryViewModelSimpleTableBinder : ISimpleCellBinder<SuggestionListSummaryViewModel>
	{
		public void Bind(SimpleCell cell, SuggestionListSummaryViewModel model)
		{
			cell.Bind(model.Name, null, null);

		}
	}
}

