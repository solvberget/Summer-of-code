using System;
using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class BlogPostViewModel : BaseViewModel
    {
        private readonly IBlogService _blogService;

        public BlogPostViewModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public void Init(string blogId, string postId)
        {
            Load(blogId, postId);
        }

        private async Task Load(string blogId, string postId)
        {
            IsLoading = true;

            var blogPost = await _blogService.GetBlogPost(blogId, postId);
            Author = blogPost.Author;
            Published = blogPost.Published;
            Content = blogPost.Content;
            Title = blogPost.Title;

            IsLoading = false;
        }

        private string _author;
        public string Author 
        {
            get { return _author; }
            set { _author = value; RaisePropertyChanged(() => Author);}
        }

        private DateTime _published;
        public DateTime Published 
        {
            get { return _published; }
            set { _published = value; RaisePropertyChanged(() => Published);}
        }

        private string _description;
        public string Description 
        {
            get { return _description; }
			set { _description = value; RaisePropertyChanged(() => Description);}
        }

        private string _content;
        public string Content 
        {
            get { return _content; }
            set { _content = value; RaisePropertyChanged(() => Content);}
        }

        private string _url;
        public string Url 
        {
            get { return _url; }
            set { _url = value; RaisePropertyChanged(() => Url);}
        }
    }
}