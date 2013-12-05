// --------------------------------------------------------------------------------------------------------------------
// <summary>
//    Defines the Main type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");

			UILabel.Appearance.Font = UIFont.FromName("Open Sans", 14);

			var menu = UITableView.AppearanceWhenContainedIn(typeof(HomeView));

			menu.BackgroundColor = UIColor.Red;	
			//menu.BackgroundColor = UIColor.FromRGB(52, 180, 69);

			UILabel.AppearanceWhenContainedIn(typeof(HomeView)).TextColor = UIColor.White;


        }

		public static class ThemeColors
		{
			public static UIColor Main = UIColor.FromRGB(52, 180, 69);
			public static UIColor Main2 = UIColor.FromRGB(51,51,51);

			public static UIColor MainInverse = UIColor.White;

			public static UIColor VerySubtle = UIColor.FromRGB(244,244,244);
			public static UIColor Subtle = UIColor.FromRGB(206, 206, 206);

			public static UIColor Hero = UIColor.FromRGB(240,251,235);
		}
    }
}