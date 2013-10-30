using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.ViewModels
{
    public class SuggestionsListViewModel : BaseViewModel
    {
        private readonly ISuggestionsService _suggestionsService;

        public SuggestionsListViewModel(ISuggestionsService suggestionsService)
        {
            _suggestionsService = suggestionsService;
        }

        public void Init(string Name)
        {
            this.Name = Name;
            Title = Name;
            Load();
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        private List<Document> _documents;
        public List<Document> Documents
        {
            get { return _documents; }
            set { _documents = value; RaisePropertyChanged(() => Documents); }
        }

        private List<SearchResultViewModel> _docs;
        public List<SearchResultViewModel> Docs
        {
            get { return _docs; }
            set { _docs = value; RaisePropertyChanged(() => Docs); }
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
        public async void Load()
        {
            //var libList = await _suggestionsService.GetList("");
            //Documents = libList;
            IsLoading = true;
            Docs = (from n in await  _suggestionsService.GetList("")
                    select new SearchResultViewModel
                           {
                               Title = n.Title,
                               Type = n.DocType,
                               Year = n.PublishedYear.ToString(),
                               DocNumber = n.DocumentNumber,
                           }).ToList();
            IsLoading = false;

        }
    }
}
