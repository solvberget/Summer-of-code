using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Solvberget.Core.ViewModels;
using Solvberget.Core.DTOs;

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

		public static Action<ISimpleCell, FavoriteViewModel> Favorites = (sc, model) =>
		{
			sc.Bind(model.Name, model.PresentableTypeWithYear); 
			sc.ImageUrl = model.Image;
		};

		public static Action<ISimpleCell, LoanViewModel> Loans = (sc, model) =>
		{
			sc.Bind(model.DocumentTitle, "Leveringsfrist: " + model.DueDate); 
			sc.ImageUrl = model.Image;
		};

		public static Action<ISimpleCell, ReservationViewModel> Reservations = (sc, model) =>
		{
			var info = model.ReadyForPickup ? "Klar for henting. Hentefrist: " + model.PickupDeadline
			           						: "Ikke klar.";

			sc.Bind(model.DocumentTitle, info); 
			sc.ImageUrl = model.Image;
		};

		public static Action<ISimpleCell, NotificationDto> Messages = (sc, model) =>
		{
			sc.Bind(model.Title, model.DocumentTitle); 
			sc.SetImage(UIImage.FromBundle("/Images/Placeholders/Message.png"));
		};

		public static Action<ISimpleCell, FineViewModel> Fines = (sc, model) =>
		{
			sc.Bind(model.Description, model.Sum + " | " + model.DocumentTitle); 
			sc.SetImage(UIImage.FromBundle("/Images/Placeholders/Fine.png"));
		};

	}
}
