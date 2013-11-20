using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace Solvberget.iOS
{
	public partial class OpeningHoursLocationCell : MvxTableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("OpeningHoursLocationCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("OpeningHoursLocationCell");

        public OpeningHoursLocationCell(IntPtr handle) : base(handle)
        {
        }

        public static OpeningHoursLocationCell Create()
        {
            return (OpeningHoursLocationCell)Nib.Instantiate(null, null)[0];
        }
    }
}

