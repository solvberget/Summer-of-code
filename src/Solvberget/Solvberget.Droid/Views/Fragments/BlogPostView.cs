using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Java.Text;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class BlogPostView : MvxFragment
    {
        private LoadingIndicator _loadingIndicator;

        private BlogPostViewModel _viewModel;
        public new BlogPostViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as BlogPostViewModel); }
        }

        public BlogPostView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreateView(inflater, container, savedInstanceState);
            _loadingIndicator = new LoadingIndicator(Activity);

            var set = this.CreateBindingSet<BlogPostView, BlogPostViewModel>();
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);
            set.Apply();

            var view = this.BindingInflate(Resource.Layout.fragment_blogpost, null);


            //ViewModel.PropertyChanged += (sender, args) =>
            //{
            //    if (args.PropertyName == "Content")
            //    {
            //        var contentView = view.FindViewById(Resource.Id.blogText) as TextView;
            //        if (contentView != null)
            //        {
            //            var formattedText = Html.FromHtml(ViewModel.Content, new HttpImageGetter(), null);
            //            contentView.TextFormatted = formattedText;
            //        }
            //    }
            //};

            return view;
        }
    }

    class HttpImageGetter : Html.IImageGetter {

        public HttpImageGetter()
        {
            
        }

        public Drawable GetDrawable(string source)
        {
            Drawable drawable;
            Bitmap bitMap;
            BitmapFactory.Options bitMapOption;
            try
            {
                bitMapOption = new BitmapFactory.Options();
                bitMapOption.InJustDecodeBounds = false;
                bitMapOption.InPreferredConfig = Bitmap.Config.Argb4444;
                bitMapOption.InPurgeable = true;
                bitMapOption.InInputShareable = true;
                var url = new Java.Net.URL(source);

                bitMap = BitmapFactory.DecodeStream(url.OpenStream(), null, bitMapOption);
                drawable = new BitmapDrawable(bitMap);
            }
            catch (Exception e)
            {
                return null;
            }

            drawable.SetBounds(0, 0, bitMapOption.OutWidth, bitMapOption.OutHeight);
            return drawable;
        }

        public void Dispose()
        {
        }

        public IntPtr Handle { get; private set; }
    }
}