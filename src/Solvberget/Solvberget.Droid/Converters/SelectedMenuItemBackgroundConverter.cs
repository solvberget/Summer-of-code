using System;
using System.Globalization;
using Android.Graphics;
using Cirrious.CrossCore.Converters;

namespace Solvberget.Droid.Converters
{
    public class SelectedMenuItemBackgroundConverter : MvxValueConverter<bool, Color>
    {
        protected override Color Convert(bool selectedValue, Type targetType, object parameter, CultureInfo culture)
        {
            return selectedValue ? new Color(0x34, 0xB4, 0x45) : new Color(0x00, 0x00, 0x00, 0x00);
        }
    }
}