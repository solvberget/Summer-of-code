using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public static class CellBindings
	{
		public static Action<ISimpleCell, EventViewModel> Events = (sc, model) =>
		{
			sc.Bind(model.Title, model.TimeAndPlaceSummary);
			sc.SetImage(UIHelpers.ImageFromUrl(model.ImageUrl));
		};

		public static Action<ISimpleCell, SuggestionListSummaryViewModel> SuggestionLists = (sc, model) =>
		{
			sc.Bind(model.Name, null);
		};

		public static Action<ISimpleCell, BlogItemViewModel> Blogs = (sc, model) =>
		{
			sc.Bind(model.Title, null);
			sc.SetImage(UIImage.FromBundle("/Images/Placeholders/Blog.png"));
		};

		public static Action<ISimpleCell, SearchResultViewModel> SearchResults = (sc, model) =>
		{
			sc.Bind(model.Name, model.PresentableTypeWithYear); 
			sc.ImageUrl = model.Image;
		};

	}
}
