using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Core.Properties;

namespace Solvberget.Core.ViewModels
{
    public class SuggestionsListViewModel : BaseViewModel
    {
        private readonly ISuggestionsService _suggestionsService;

        public SuggestionsListViewModel(ISuggestionsService suggestionsService)
        {
            _suggestionsService = suggestionsService;
        }

        public void Init(string name, string id)
        {
            Name = name;
            Id = id;
            Title = name;
            Load();
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        private string _id;
        public new string Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(() => Id); }
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
			ShowViewModel<MediaDetailViewModel>(new { docId = searchResultViewModel.DocNumber, title = searchResultViewModel.Title});
        }

        // Loads a a set of Documents retrieved from the service into the results list.
        public async void Load()
        {
            IsLoading = true;
            var list = await _suggestionsService.GetList(Id);
            Docs = (from n in list.Documents
                    select new SearchResultViewModel
                           {
                               Name = n.Title,
                               Type = n.Type,
                               Year = n.Year.ToString(),
                               DocNumber = n.Id,
								Title = n.SubTitle,
					Image = Resources.ServiceUrl + string.Format(Resources.ServiceUrl_MediaImage, n.Id),
                           }).ToList();
            IsLoading = false;
        }
    }
}
