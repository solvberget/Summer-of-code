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

			var source = new StandardTableViewSource(TableView, UITableViewCellStyle.Default,
				"MyPageSections", "TitleText Title");

			var sections = new List<MyPageSection>();
			sections.Add(new MyPageSection{ Title = "Favoritter", ViewModel = ViewModel.MyPageFavoritesViewModel });
			sections.Add(new MyPageSection{ Title = "Lån", ViewModel = ViewModel.MyPageLoansViewModel });
			sections.Add(new MyPageSection{ Title = "Reservasjoner", ViewModel = ViewModel.MyPageReservationsViewModel });
			sections.Add(new MyPageSection{ Title = "Gebyrer", ViewModel = ViewModel.MyPageFinesViewModel });
			sections.Add(new MyPageSection{ Title = "Meldinger", ViewModel = ViewModel.MyPageMessagesViewModel });
			sections.Add(new MyPageSection{ Title = "Min profil", ViewModel = ViewModel.MyPagePersonaliaViewModel });

			source.ItemsSource = sections;
			source.SelectedItemChanged += (s, e) =>
			{
				var item = (MyPageSection)source.SelectedItem;
				var ctl = this.CreateViewControllerFor(sections.First(se => se.Title == item.Title).ViewModel) as UIViewController;
				this.NavigationController.PushViewController(ctl, true);
			};

			TableView.Source = source;
		}
       
    }
}

