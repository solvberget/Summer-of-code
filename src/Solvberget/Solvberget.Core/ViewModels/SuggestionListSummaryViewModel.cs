using System.Collections.Generic;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.ViewModels
{
    public class SuggestionListSummaryViewModel : BaseViewModel
    {
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

        private string _id;
        public new string Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(() => Id); }  
        }
    }
}
