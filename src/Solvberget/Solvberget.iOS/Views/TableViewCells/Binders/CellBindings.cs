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
		public static Action<UITableViewCell, EventViewModel> Events = (cell, model) =>
		{
			var sc = (SimpleCell)cell;
			sc.Bind(model.Title, model.TimeAndPlaceSummary, model.ImageUrl);
		};

		public static Action<UITableViewCell, SuggestionListSummaryViewModel> SuggestionLists = (cell, model) =>
		{
			var sc = (SimpleCell)cell;
			sc.Bind(model.Title, null, null);
		};

		public static Action<UITableViewCell, BlogItemViewModel> Blogs = (cell, model) =>
		{
			var sc = (SimpleCell)cell;
			sc.Bind(model.Title, model.Description, null);
		};

		public static Action<UITableViewCell, SearchResultViewModel> SearchResults = (cell, model) =>
		{
			var sc = (SimpleCell)cell;
			sc.Bind(model.Name, model.PresentableTypeWithYear, model.Image);
		};

		public static Action<UITableViewCell, BlogPostViewModel> BlogPosts = (cell, model) =>
		{

		};
	}
}
