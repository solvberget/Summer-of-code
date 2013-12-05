using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using System;
using System.Threading;

namespace Solvberget.Core.ViewModels
{
    public class BlogViewModel : BaseViewModel
    {
		AutoResetEvent _viewModelReady = new AutoResetEvent(false);

		public void WaitForReady(Action onReady)
		{
			ThreadPool.QueueUserWorkItem(s =>
			{
				_viewModelReady.WaitOne();
				onReady();
			});
		}

        private readonly IBlogService _blogService;

        public BlogViewModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

		public void Init(long id, string title)
        {
			Title = title;
            Load(id);
        }

        private async Task Load(long id)
        {
            IsLoading = true;
            Id = id;
            
            Posts = (await _blogService.GetBlogPostListing(id)).Posts.Select(p => new BlogPostViewModel(_blogService)
            {
                Id = p.Id,
                Author = p.Author,
                Content = p.Description,
                Description = p.Description,
                Title = p.Title,
                Published = p.Published,
                Url = "", // TODO: Do we want blog post urls?
            }).ToList();

            IsLoading = false;
			_viewModelReady.Set();
        }

        private List<BlogPostViewModel> _posts;
        public List<BlogPostViewModel> Posts 
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

        private MvxCommand<BlogPostViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<BlogPostViewModel>(ExecuteShowDetailsCommand));
            }
        }

        private async void ExecuteShowDetailsCommand(BlogPostViewModel post)
        {
            IsLoading = true;

            var blogPost = await _blogService.GetBlogPost(Id.ToString(), post.Id.ToString());
            var html = blogPost.Content;
            var title = blogPost.Title;

            IsLoading = false;

            ShowViewModel<LocalHtmlWebViewModel>(new { html = html, title = title });
        }
    }
}