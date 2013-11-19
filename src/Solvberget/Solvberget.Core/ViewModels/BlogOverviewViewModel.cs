using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class BlogOverviewViewModel : BaseViewModel
    {
        private readonly IBlogService _blogService;

        public BlogOverviewViewModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public void Init()
        {
			Title = "Blogger";
            Load();
        }

        private async Task Load()
        {
            IsLoading = true;
            Blogs = (await _blogService.GetBlogListing()).Select(b => new BlogItemViewModel
            {
                Description = b.Description,
                Id = b.Id,
                Title = b.Title
            }).ToList();
            IsLoading = false;
        }

        private List<BlogItemViewModel> _blogs;
        public List<BlogItemViewModel> Blogs 
        {
            get { return _blogs; }
            set { _blogs = value; RaisePropertyChanged(() => Blogs);}
        }

        private MvxCommand<BlogItemViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<BlogItemViewModel>(ExecuteShowDetailsCommand));
            }
        }

        private void ExecuteShowDetailsCommand(BlogItemViewModel blog)
        {
            ShowViewModel<BlogViewModel>(new { id = blog.Id });
        }
    }
}
