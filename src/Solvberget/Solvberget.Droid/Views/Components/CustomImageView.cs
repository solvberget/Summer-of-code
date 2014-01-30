using Android.Content;
using Android.Util;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Solvberget.Droid.Views.Components
{
    class CustomImageView : MvxImageView
    {
        public CustomImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomImageView(Context context) : base(context)
        {
        }
    }
}