using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class SuggestionsListListViewModel : BaseViewModel
    {
        private readonly ISuggestionsService _suggestionsService;

        public SuggestionsListListViewModel(ISuggestionsService suggestionsService)
        {
            _suggestionsService = suggestionsService;
            Title = "Anbefalinger";
        }

        public void Init()
        {
            Load();
        }

        private List<SuggestionListSummaryViewModel> _lists;
        public List<SuggestionListSummaryViewModel> Lists
        {
            get { return _lists; }
            set { _lists = value; RaisePropertyChanged(() => Lists); }
        }

        private MvxCommand<SuggestionListSummaryViewModel> _showListCommand;
        public ICommand ShowListCommand
        {
            get
            {
                return _showListCommand ?? (_showListCommand = new MvxCommand<SuggestionListSummaryViewModel>(ExecuteShowListCommand));
            }
        }

        private void ExecuteShowListCommand(SuggestionListSummaryViewModel suggestionsListSummary)
        {
            ShowViewModel<SuggestionsListViewModel>(new { suggestionsListSummary.Name, suggestionsListSummary.Id });
        }

        // Loads a a set of Lists retrieved from the service into the results list.
        public async void Load()
        {
            IsLoading = true;

            Lists = (from n in await _suggestionsService.GetSuggestionsLists()
                     select new SuggestionListSummaryViewModel
                           {
                               Name = n.Name,
                               Id = n.Id
                           }).ToList();

            IsLoading = false;
        }
    }
}
