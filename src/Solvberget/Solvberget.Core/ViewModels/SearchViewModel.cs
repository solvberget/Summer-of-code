using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class SearchViewModel : BaseViewModel 
    {
        private readonly ISearchService _searchService;

        public SearchViewModel(ISearchService searchService)
        {
            _searchService = searchService;
            Title = "Søk";
        }

        private string _query;
        public string Query 
        {
            get { return _query; }
            set { _query = value; RaisePropertyChanged(() => Query);}
        }

        private IList<SearchResultViewModel> _results;
        public IList<SearchResultViewModel> Results 
        {
            get { return _results; }
            set { _results = value; RaisePropertyChanged(() => Results);}
        }

        private MvxCommand<SearchResultViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<SearchResultViewModel>(ExecuteShowDetailsCommand));
            }
        }

        private void ExecuteShowDetailsCommand(SearchResultViewModel searchResultViewModel)
        {
            ShowViewModel<MediaDetailViewModel>(searchResultViewModel.DocNumber);
        }

        // Loads a a set of Documents retrieved from the service into the results list.
        public void SearchAndLoad()
        {
            var results = _searchService.Search(Query);
            Results = (from document in results
                select new SearchResultViewModel()
                {
                    Name = document.Title,
                    Type = document.DocType,
                    Year = document.PublishedYear.ToString("0000"),
                    DocNumber = document.DocumentNumber
                }).ToList();
        }
    }
}
