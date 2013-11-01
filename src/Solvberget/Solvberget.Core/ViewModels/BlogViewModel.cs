using System.Collections.Generic;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class BlogViewModel : BaseViewModel
    {
        private BlogPostViewModel _posts;
        public BlogPostViewModel Posts 
        {
            get { return _posts; }
            set { _posts = value; RaisePropertyChanged(() => Posts);}
        }

        private string _url;
        public string Url 
        {
            get { return _url; }
            set { _url = value; RaisePropertyChanged(() => Url);}
        }

        private string _description;
        public string Description 
        {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(() => Description);}
        }

        private List<string> _tags;
        public List<string> Tags 
        {
            get { return _tags; }
            set { _tags = value; RaisePropertyChanged(() => Tags);}
        }
    }
}