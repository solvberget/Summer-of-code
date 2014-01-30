using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;

namespace Solvberget.Droid.Views.Components
{
    public class BackgroundColorBindableTextView : TextView
    {
        public BackgroundColorBindableTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                SetBackgroundColor(value);
            }
        }
    }
}