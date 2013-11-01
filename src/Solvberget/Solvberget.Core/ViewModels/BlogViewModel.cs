using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class BlogViewModel : BaseViewModel
    {
        public void Init(string id)
        {
            Load();
        }

        private async Task Load()
        {
            // TODO: Implement
            IsLoading = true;
            await TaskEx.Delay(500);

            Posts = new List<BlogPostViewModel>
            {
                new BlogPostViewModel {Author = "Ole Olsen", Description = "Les denne spennende posten", Published = DateTime.Today, Id = 1},
                new BlogPostViewModel {Author = "Ole Olsen", Description = "Les denne spennende posten", Published = DateTime.Today, Id = 2},
                new BlogPostViewModel {Author = "Ole Olsen", Description = "Les denne spennende posten", Published = DateTime.Today, Id = 3}
            };

            IsLoading = false;
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

        private void ExecuteShowDetailsCommand(BlogPostViewModel post)
        {
            ShowViewModel<BlogPostViewModel>(new { id = post.Id });
        }
    }
}