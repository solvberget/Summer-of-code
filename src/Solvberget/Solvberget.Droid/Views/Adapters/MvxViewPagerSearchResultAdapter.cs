using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Support.V4.App;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Droid.Views.Fragments;

namespace Solvberget.Droid.Views.Adapters
{
    public class MvxViewPagerSearchResultFragmentAdapter : FragmentPagerAdapter
    {
        public class SearchResultFragmentInfo
        {
            public string Title { get; set; }
            public IMvxViewModel ViewModel { get; set; }
            public string BindableProperty { get; set; }
        }

        private readonly Context _context;

        public MvxViewPagerSearchResultFragmentAdapter(
          Context context, FragmentManager fragmentManager, IEnumerable<SearchResultFragmentInfo> fragments)
            : base(fragmentManager)
        {
            _context = context;
            Fragments = fragments;
        }

        public IEnumerable<SearchResultFragmentInfo> Fragments { get; private set; }

        public override int Count
        {
            get { return Fragments.Count(); }
        }

        public override Fragment GetItem(int position)
        {
            var frag = Fragments.ElementAt(position);
            var fragment = Fragment.Instantiate(_context, FragmentJavaName(typeof(SearchResultCategoryView))) as SearchResultCategoryView;
            fragment.DataContext = frag.ViewModel;
            fragment.BindableProperty = frag.BindableProperty;
            
            return fragment;
        }

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            var namespaceText = fragmentType.Namespace ?? "";
            if (namespaceText.Length > 0)
                namespaceText = namespaceText.ToLowerInvariant() + ".";
            return namespaceText + fragmentType.Name;
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int p0) { return new Java.Lang.String(Fragments.ElementAt(p0).Title); }
    }
}