using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Solvberget.iOS
{
	public class StandardTableViewSource : MvxStandardTableViewSource
	{
		public StandardTableViewSource(UITableView tableView, string bindingText)
			: base(tableView, bindingText)
		{
		}

		public StandardTableViewSource(UITableView tableView, UITableViewCellStyle style, string cellIdentifier,  string bindingText, UITableViewCellAccessory accessory = UITableViewCellAccessory.DisclosureIndicator) 
			: base(tableView, style, new NSString(cellIdentifier), bindingText, accessory)
		{
		}

		protected override MvxStandardTableViewCell CreateDefaultBindableCell(UITableView tableView, NSIndexPath indexPath, object item)
		{
			var cell = base.CreateDefaultBindableCell(tableView, indexPath, item);

			cell.TextLabel.Font = Application.ThemeColors.DefaultFont;
			cell.TextLabel.TextColor = Application.ThemeColors.Main2;

			if (null != cell.DetailTextLabel)
			{
				cell.DetailTextLabel.Font = Application.ThemeColors.DefaultFont;
				cell.DetailTextLabel.TextColor = Application.ThemeColors.Subtle;
			}

			return cell;
		}
	}
}

