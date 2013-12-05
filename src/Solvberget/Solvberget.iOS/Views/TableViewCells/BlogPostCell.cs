using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace Solvberget.iOS
{
	public partial class BlogPostCell : MvxTableViewCell
	{
		public static readonly UINib Nib = UINib.FromName("BlogPostCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString("BlogPostCell");

		public BlogPostCell(IntPtr handle) : base(handle)
		{
		}

       
    }
}

