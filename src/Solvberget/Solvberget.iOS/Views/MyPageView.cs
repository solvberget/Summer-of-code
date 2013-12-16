using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Views;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Linq;

namespace Solvberget.iOS
{
	public class MyPageSection
	{
		public string Title {get;set;}
		public IMvxViewModel ViewModel {get;set;}
	}

	public partial class MyPageView : NamedTableViewController
    {
        public MyPageView()
		{
        }

		private new MyPageViewModel ViewModel { get { return base.ViewModel as MyPageViewModel; } }


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Logg ut", UIBarButtonItemStyle.Plain, DoLogout), false);


			LoadingOverlay.LoadingText = "Henter din side...";

			var source = new StandardTableViewSource(TableView, UITableViewCellStyle.Default,
				"MyPageSections", "TitleText Title");

			var sections = new List<MyPageSection>();
			sections.Add(new MyPageSection{ Title = "Favoritter", ViewModel = ViewModel.MyPageFavoritesViewModel });
			sections.Add(new MyPageSection{ Title = "Lån", ViewModel = ViewModel.MyPageLoansViewModel });
			sections.Add(new MyPageSection{ Title = "Reservasjoner", ViewModel = ViewModel.MyPageReservationsViewModel });
			sections.Add(new MyPageSection{ Title = "Gebyrer", ViewModel = ViewModel.MyPageFinesViewModel });

			var messagesSection = new MyPageSection{ Title = "Meldinger (..)", ViewModel = ViewModel.MyPageMessagesViewModel };
			sections.Add(messagesSection);
			sections.Add(new MyPageSection{ Title = "Min profil", ViewModel = ViewModel.MyPagePersonaliaViewModel });

			ViewModel.MyPageMessagesViewModel.WaitForReady(() =>
			{
					InvokeOnMainThread(() => 
					{
						messagesSection.Title = String.Format("Meldinger ({0})", ViewModel.MyPageMessagesViewModel.Notifications.Count);

						TableView.ReloadRows(TableView.IndexPathsForVisibleRows, UITableViewRowAnimation.Fade);
					});
			});

			source.ItemsSource = sections;
			source.SelectedItemChanged += (s, e) =>
			{
				var item = (MyPageSection)source.SelectedItem;
				var ctl = this.CreateViewControllerFor(sections.First(se => se.Title == item.Title).ViewModel) as UIViewController;
				this.NavigationController.PushViewController(ctl, true);
			};

			TableView.Source = source;
		}

		private void DoLogout(object sender, EventArgs args)
		{
			ViewModel.Logout();
		}
       
    }
}

