using System;
using System.Globalization;
using Android.Graphics;
using Cirrious.CrossCore.Converters;

namespace Solvberget.Droid.Converters
{
    public class StringToColorConverter : MvxValueConverter<string, Color>
    {
        protected override Color Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Min Side":
                    return new Color(0x34, 0xB4, 0x45);
                case "Arrangementer":
                    return new Color(0x17, 0x8F, 0x4F);
                case "Søk":
                    return new Color(0x00, 0x74, 0x13);
                case "Blogger":
                    return new Color(0x34, 0xB4, 0x45);
                case "Nyheter":
                    return new Color(0x34, 0xB4, 0x45);
                case "Anbefalinger":
                    return new Color(0x66, 0xD0, 0x28);
                case "Åpningstider":
                    return new Color(0x34, 0xB4, 0x45);
                case "Kontakt oss":
                    return new Color(0x34, 0xB4, 0x45);
                default:
                    return new Color(0x34, 0xB4, 0x45);
            }
        }
    }
}