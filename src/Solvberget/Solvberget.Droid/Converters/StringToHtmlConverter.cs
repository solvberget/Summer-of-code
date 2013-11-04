using System;
using System.Globalization;
using Android.Text;
using Cirrious.CrossCore.Converters;

namespace Solvberget.Droid.Converters
{
    public class StringToHtmlConverter : MvxValueConverter<string, ISpanned>
    {
        protected override ISpanned Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return Html.FromHtml(value);
        }
    }
}