using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.DTOs;

namespace Solvberget.Droid.Views.Components
{
    public class MediaDetailInformationView : LinearLayout, IMvxBindingContextOwner
    {
        private readonly IMvxBindingContext _bindingContext;

        public object DataContext
        {
            get { return _bindingContext.DataContext; }
            set
            {
                if (value != null)
                {
                    _bindingContext.DataContext = value;

                    View view = null;

                    if (DataContext is BookDto)
                    {
                        view = this.BindingInflate(Resource.Layout.mediadetail_book, null);
                    }
                    else if (DataContext is FilmDto)
                    {
                        view = this.BindingInflate(Resource.Layout.mediadetail_film, null);
                    }
                    else if (DataContext is CdDto)
                    {
                        view = this.BindingInflate(Resource.Layout.mediadetail_cd, null);
                    }
                    else if (DataContext is SheetMusicDto)
                    {
                        view = this.BindingInflate(Resource.Layout.mediadetail_sheetmusic, null);
                    }
                    else if (DataContext is GameDto)
                    {
                        view = this.BindingInflate(Resource.Layout.mediadetail_game, null);
                    }
                    else if (DataContext is JournalDto)
                    {
                        view = this.BindingInflate(Resource.Layout.mediadetail_journal, null);
                    }

                    if (view != null)
                    {
                        AddView(view);
                    }
                }
            }
        }

        public MediaDetailInformationView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            var inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            _bindingContext = new MvxAndroidBindingContext(context, new MvxSimpleLayoutInflater(inflater));
        }

        public IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
            set { throw new NotImplementedException(); }
        }
    }
}