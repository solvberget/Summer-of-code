using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services;
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
            Load();
        }

        private string _query;
        public string Query 
        {
            get { return _query; }
            set { _query = value; RaisePropertyChanged(() => Query);}
        }

        private IEnumerable<SearchResultViewModel> _results;
        public IEnumerable<SearchResultViewModel> Results 
        {
            get { return _results; }
            set { _results = value; RaisePropertyChanged(() => Results);}
        }

        private MvxCommand<SearchResultViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<SearchResultViewModel>((vm) => {}));
            }
        }

        public void Load()
        {
            var results = _searchService.Search(Query);
            Results = from document in results
                select new SearchResultViewModel()
                {
                    Name = document.Title
                };
             Mvx.Trace("Results #: " + Results.Count());
        }


    }

    public class SearchResultViewModel : BaseViewModel
    {
        private string _name;
        public string Name 
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name);}
        }
    }
}
