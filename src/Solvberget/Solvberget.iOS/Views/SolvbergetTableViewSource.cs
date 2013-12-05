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

	public class SimpleTableViewSource : UITableViewSource
	{
		public SimpleTableViewSource()
			: base()
		{}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			//var cell = tableView.DequeueReusableCell("SimpleTableCell") as SimpleTableCell;

			//if(null == cell) cell = new SimpleTableCell("SimpleTableCell");

			//cell.TextLabel.Text = "Hi there!";

			return null;
		}

		#region implemented abstract members of UITableViewSource

		public override int RowsInSection(UITableView tableview, int section)
		{
			return 0;
		}

		#endregion
	}

	public class SolvbergetTableViewSource : MvxStandardTableViewSource
	{
		public SolvbergetTableViewSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, string bindingText, UITableViewCellAccessory tableViewCellAccessory = 0)
			: base(tableView, style, cellIdentifier, bindingText, tableViewCellAccessory)
		{}
			
		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		{
			UITableViewCell cell = base.GetOrCreateCellFor(tableView, indexPath, item);

			cell.BackgroundView = null;

			if(null != BackgroundColor) cell.BackgroundColor = BackgroundColor;
			if(null != TextColor) cell.TextLabel.TextColor = TextColor;
			if(null != TintColor) cell.SelectedBackgroundView = SelectedBackgroundView;

			return cell;
		}

		UIView _selectedBackgroundView;

		public UIView SelectedBackgroundView{
			get{ 
				if (null == _selectedBackgroundView && null != TintColor)
				{
					_selectedBackgroundView = new UIView();
					_selectedBackgroundView.BackgroundColor = TintColor;
					_selectedBackgroundView.Layer.MasksToBounds = true;
				}

				return _selectedBackgroundView;
			}
		}

		public UIColor BackgroundColor{get;set;}
		public UIColor TextColor{ get; set;}
		public UIColor TintColor{ get; set;}
	}
}
