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
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.Threading;

namespace Solvberget.iOS
{
	public static class Cache{

		public static Dictionary<string, UIImage> Images = new Dictionary<string, UIImage>();
	}
	
}
