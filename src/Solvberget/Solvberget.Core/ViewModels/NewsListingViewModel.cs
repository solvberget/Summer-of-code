using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using System.Linq;

namespace Solvberget.Core.ViewModels
{
    public class NewsListingViewModel : BaseViewModel 
    {
        private readonly INewsService _newsService;

        public NewsListingViewModel(INewsService newsService)
        {
            _newsService = newsService;
            Title = "Nyheter";
        }

        public void Init()
        {
            Load();
        }

        private IList<NewsViewModel> _stories;
        public IList<NewsViewModel> Stories 
        {
            get { return _stories; }
            set { _stories = value; RaisePropertyChanged(() => Stories);}
        }

        private MvxCommand<NewsViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<NewsViewModel>(ExecuteShowDetailsCommand));
            }
        }

        private void ExecuteShowDetailsCommand(NewsViewModel newsStory)
        {
            ShowViewModel<GenericWebViewViewModel>(new {uri = newsStory.Uri.ToString(), title = newsStory.NewsTitle});
        }

        // Loads a a set of Documents retrieved from the service into the results list.
        public async void Load()
        {
            IsLoading = true;

            Stories = (from n in await _newsService.GetNews()
                select new NewsViewModel
                {
                    Ingress = n.Ingress,
                    Uri = n.Link,
                    Published = n.Published,
                    NewsTitle = n.Title
                }).ToList();

			IsLoading = false;
			NotifyViewModelReady();
        }
    }
}
