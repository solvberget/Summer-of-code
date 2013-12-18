using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class LocalHtmlWebViewModel : BaseViewModel
    {
        public void Init(string html, string title)
        {
            Html = html;
            Title = title;
        }

        private string _html;
        public string Html 
        {
            get { return _html; }
            set { _html = value; RaisePropertyChanged(() => Html);}
        }
    }
}