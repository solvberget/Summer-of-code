using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
    public partial class MenuCell : UITableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("MenuCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("MenuCell");

        public MenuCell(IntPtr handle) : base(handle)
        {
        }

        public static MenuCell Create()
        {
            return (MenuCell)Nib.Instantiate(null, null)[0];
        }

		private void Style()
		{
			_iconLabel.Font = Application.ThemeColors.IconFont.WithSize(20);
			_iconLabel.TextColor = Application.ThemeColors.Hero;

			var selectedBackgroundView = new UIView();
			selectedBackgroundView.BackgroundColor = Application.ThemeColors.Main;
			selectedBackgroundView.Layer.MasksToBounds = true;

			BackgroundColor = Application.ThemeColors.Main2;
			_titleLabel.TextColor = Application.ThemeColors.MainInverse;
			_titleLabel.Font = Application.ThemeColors.MenuFont;

			BackgroundView = null;
			SelectedBackgroundView = selectedBackgroundView;

			_styled = true;
		}

		bool _styled;

		public void Set(MenuViewModel item)
		{
			if (!_styled)
				Style();

			_iconLabel.Text = item.IconChar;
			_titleLabel.Text = item.Title;
		}
    }
}

