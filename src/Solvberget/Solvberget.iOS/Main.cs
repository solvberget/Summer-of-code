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
            UIApplication.Main(args, null, "AppDelegate");
        }

		public static class ThemeColors
		{
			public static UIColor Main = UIColor.FromRGB(52, 180, 69);
			public static UIColor Main2 = UIColor.FromRGB(51,51,51);

			public static UIColor MainInverse = UIColor.White;

			public static UIColor VerySubtle = UIColor.FromRGB(244,244,244);
			public static UIColor Subtle = UIColor.FromRGB(206, 206, 206);

			public static UIColor Hero = UIColor.FromRGB(240,251,235);

			public static UIFont TitleFont = UIFont.FromName("OpenSans-Light", 16);
			public static UIFont DefaultFont = UIFont.FromName("OpenSans", 13);

			public static UIFont HeaderFont = UIFont.FromName("OpenSans-Bold", 16);
		}
    }
}