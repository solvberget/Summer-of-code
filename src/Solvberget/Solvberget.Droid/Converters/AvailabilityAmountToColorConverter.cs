using System;
using System.Globalization;
using Android.Graphics;
using Cirrious.CrossCore.Converters;

namespace Solvberget.Droid.Converters
{
    public class AvailabilityAmountToColorConverter : MvxValueConverter<int, Color>
    {
        protected override Color Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value > 0) ? new Color(0x34, 0xB4, 0x45) : new Color(0xFF, 0x99, 0x00);
        }
    }
}