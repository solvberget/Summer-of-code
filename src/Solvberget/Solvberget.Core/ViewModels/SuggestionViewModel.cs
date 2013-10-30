using Solvberget.Core.ViewModels.Base;

//This View Model is for a sigle document as a part of a list.
//It will not be used, as there already are a ViewModel for this, SearchResultViewModel. Will use that in stead.

namespace Solvberget.Core.ViewModels
{
    public class SuggestionViewModel : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; RaisePropertyChanged(() => Type); }
        }

        private string _year;
        public string Year
        {
            get { return _year; }
            set { _year = value; RaisePropertyChanged(() => Year); }
        }

        private string _docNumber;
        public string DocNumber
        {
            get { return _docNumber; }
            set { _docNumber = value; RaisePropertyChanged(() => DocNumber); }
        }
    }
}
