using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Android.Opengl;
using Android.Views;
using Cirrious.CrossCore.Converters;

namespace Solvberget.Droid.Converters
{

    public class ListAmountToVisibilityConverter : MvxValueConverter
    {
        protected object Convert(object value, object targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return ViewStates.Gone;
            return (((ICollection<View>)value).Count > 0) ? ViewStates.Visible : ViewStates.Gone;
        }
    }

    public class InvertedListAmountToVisibilityConverter : MvxValueConverter
    {
        protected object Convert(object value, object targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return ViewStates.Visible;
            return (((ICollection<View>)value).Count > 0) ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}
