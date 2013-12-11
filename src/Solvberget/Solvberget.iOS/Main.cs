// --------------------------------------------------------------------------------------------------------------------
// <summary>
//    Defines the Main type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using Solvberget.Core.ViewModels.Base;


namespace Solvberget.iOS
{
    using MonoTouch.UIKit;

    /// <summary>
    ///    Defines the Main type.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// This is the main entry point of the application.
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main(string[] args)
		{
			BaseViewModel.AddEmptyItemForEmptyLists = false;

            UIApplication.Main(args, null, "AppDelegate");

        }

		public static class ThemeColors
		{
			public static UIColor Main = UIColor.FromRGB(52, 180, 69);
			public static UIColor Main2 = UIColor.FromRGB(51,51,51);

			public static UIColor FavoriteColor = UIColor.Yellow;

			public static UIColor MainInverse = UIColor.White;

			public static UIColor ButtonBackground = Main;
			public static UIColor ButtonTextColor = MainInverse;
			public static UIColor ButtonDisabledTextColor = MainInverse.ColorWithAlpha(0.5f);

			public static UIColor VerySubtle = UIColor.FromRGB(244,244,244);
			public static UIColor Subtle = UIColor.FromRGB(206, 206, 206);
			public static UIColor SubTitleColor = UIColor.FromRGB(100, 100, 100);

			public static UIColor Hero = UIColor.FromRGB(240,251,235);

			public static UIFont MenuFont = UIFont.FromName("OpenSans", 18);
			public static UIFont TitleFont1 = UIFont.FromName("OpenSans", 22);

			public static UIFont TitleFont = UIFont.FromName("OpenSans", 16);
			public static UIFont SubTitleFont = UIFont.FromName("OpenSans", 14);

			public static UIFont DefaultFont = UIFont.FromName("OpenSans", 15);
			public static UIFont DefaultFontBold = UIFont.FromName("OpenSans-Bold", 15);
			public static UIFont LabelFont = UIFont.FromName("OpenSans-Bold", 11);	
			public static UIFont ButtonFont = UIFont.FromName("OpenSans-Bold", 15);

			public static UIFont HeaderFont = UIFont.FromName("OpenSans-Bold", 17);

			public static UIFont IconFont = UIFont.FromName("icons", 80);

			public static void Style(UIButton reserve)
			{
				reserve.SetTitleColor(Application.ThemeColors.ButtonDisabledTextColor, UIControlState.Disabled);
				reserve.SetTitleColor(Application.ThemeColors.ButtonTextColor, UIControlState.Normal);
				reserve.SetTitleColor(Application.ThemeColors.ButtonTextColor.ColorWithAlpha(0.5f), UIControlState.Selected);
				reserve.SetTitleColor(Application.ThemeColors.ButtonTextColor.ColorWithAlpha(0.5f), UIControlState.Highlighted);
				reserve.Font = Application.ThemeColors.ButtonFont;
				reserve.BackgroundColor = Application.ThemeColors.ButtonBackground;
			}
		}
    }
}