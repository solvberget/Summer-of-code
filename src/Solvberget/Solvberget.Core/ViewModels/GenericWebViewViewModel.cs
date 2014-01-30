using System;
using Cirrious.CrossCore;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class GenericWebViewViewModel : BaseViewModel
    {
        public void Init(String uri, string title)
        {
            Uri = uri;
            Title = title;
            Mvx.Trace(String.Format("Jus set Uri to {0}", Uri));
        }

        private String _uri;
        public String Uri 
        {
            get { return _uri; }
            set { _uri = value; RaisePropertyChanged(() => Uri);}
        }
    }
}