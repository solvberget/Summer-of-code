using System.Collections.Generic;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class BlogOverviewViewModel : BaseViewModel
    {
        private List<BlogViewModel> _blogs;
        public List<BlogViewModel> Blogs 
        {
            get { return _blogs; }
            set { _blogs = value; RaisePropertyChanged(() => Blogs);}
        }
    }
}
