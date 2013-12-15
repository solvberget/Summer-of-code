using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using SlidingPanels.Lib;
using System.Collections.Generic;
using SlidingPanels.Lib.PanelContainers;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace Solvberget.iOS
{

	public class MenuTableViewSource : MvxStandardTableViewSource
	{
		public MenuTableViewSource(UITableView tableView)
			: base(tableView, MenuCell.Key)
		{
			tableView.RegisterNibForCellReuse(UINib.FromName(MenuCell.Key, NSBundle.MainBundle), MenuCell.Key);
		}

		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		{
			MenuCell cell = base.GetOrCreateCellFor(tableView, indexPath, item) as MenuCell;

			MenuViewModel model = (MenuViewModel)item;

			cell.Set(model);

			_indexPathItems[item] = indexPath;

			return cell;
		}

		UIView _selectedBackgroundView;

		Dictionary<object, NSIndexPath> _indexPathItems = new Dictionary<object, NSIndexPath>();

		public void SelectRow(MenuViewModel row)
		{
			NSIndexPath path = null;

			if(null != row) _indexPathItems.TryGetValue(row, out path);

			if(null != TableView.IndexPathForSelectedRow) TableView.DeselectRow(TableView.IndexPathForSelectedRow,false);
			if(null != path) TableView.SelectRow(path, false, UITableViewScrollPosition.None);
		}

		public UIColor BackgroundColor{get;set;}
		public UIColor TextColor{ get; set;}
		public UIColor TintColor{ get; set;}
	}
}
