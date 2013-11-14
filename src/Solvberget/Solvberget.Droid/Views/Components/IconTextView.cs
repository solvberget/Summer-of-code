using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;

namespace Solvberget.Droid.Views.Components
{
    public sealed class IconTextView : TextView
    {
        public IconTextView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            var typeface = Typeface.CreateFromAsset(context.Assets, "icons.ttf");
            SetTypeface(typeface, TypefaceStyle.Normal);
        }
    }
}