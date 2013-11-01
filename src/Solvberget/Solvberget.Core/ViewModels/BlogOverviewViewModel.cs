using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class BlogOverviewViewModel : BaseViewModel
    {
        public void Init()
        {
            Load();
            
        }

        private async Task Load()
        {
            // TODO: Implement
            IsLoading = true;
            await TaskEx.Delay(500);
            Blogs = new List<BlogViewModel>
            {
                new BlogViewModel() {Description = "Hello", Title =  "Title", },
                new BlogViewModel() {Description = "Hello 2", Title =  "Title 2", },
                new BlogViewModel() {Description = "Hello 3", Title =  "Title 3", },
            };
            IsLoading = false;
        }

        private List<BlogViewModel> _blogs;
        public List<BlogViewModel> Blogs 
        {
            get { return _blogs; }
            set { _blogs = value; RaisePropertyChanged(() => Blogs);}
        }

        private MvxCommand<BlogViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<BlogViewModel>(ExecuteShowDetailsCommand));
            }
        }

        private void ExecuteShowDetailsCommand(BlogViewModel blog)
        {
            ShowViewModel<BlogViewModel>(new { id = blog.Id });
        }
    }
}
