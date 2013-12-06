using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public partial class MyPageFavoritesView : NamedTableViewController
    {
		public new MyPageFavoritesViewModel ViewModel
		{
			get
			{
				return base.ViewModel as MyPageFavoritesViewModel;
			}
		}


    }
}

