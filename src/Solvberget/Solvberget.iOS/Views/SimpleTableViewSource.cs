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
	public class SimpleTableViewSource<T> : MvxTableViewSource where T : class
	{
		Action<UITableViewCell, T> _binder;

		public SimpleTableViewSource(UITableView tableView, Action<UITableViewCell, T> binder) : base(tableView)
		{
			_binder = binder;
		
			tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			tableView.RegisterNibForCellReuse(UINib.FromName(SimpleCell.Key, NSBundle.MainBundle), SimpleCell.Key);

		}

		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 95;
		}

		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		{

			var key = GetNibNameForVM(item.GetType());

			var cell = TableView.DequeueReusableCell(key, indexPath);

			_binder(cell, item as T);

			return cell;
		}

		NSString GetNibNameForVM(Type type)
		{
			return SimpleCell.Key;
		}
	}
	
}
