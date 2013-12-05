using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{

	public class EventViewModelSimpleListBinder : ISimpleCellBinder<EventViewModel>
	{
		public void Bind(SimpleCell cell, EventViewModel model)
		{
			cell.Bind(model.Title, model.TimeAndPlaceSummary, model.ImageUrl);
		}
	}
}
