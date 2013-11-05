using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Solvberget.Droid.Views.Components
{
    public class MediaDetailInformationView : MvxLinearLayout
    {
        public MediaDetailInformationView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public MediaDetailInformationView(Context context, IAttributeSet attrs, IMvxAdapterWithChangedEvent adapter) : base(context, attrs, adapter)
        {
        }
    }
}