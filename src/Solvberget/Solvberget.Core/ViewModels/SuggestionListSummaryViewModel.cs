using System.Collections.Generic;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Core.DTOs;

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

		private List<DocumentDto> _documents;
		public List<DocumentDto> Documents
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

        private string _subTitle;
        public string SubTitle
        {
            get { return _subTitle; }
            set { _subTitle = value; RaisePropertyChanged(() => SubTitle); }
        }
    }
}
