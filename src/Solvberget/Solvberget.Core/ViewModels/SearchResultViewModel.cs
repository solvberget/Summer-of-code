using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class SearchResultViewModel : BaseViewModel
    {
        private string _name;
        public string Name 
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name);}
        }

        private string _type;
        public string Type 
        {
            get { return _type; }
            set { _type = value; RaisePropertyChanged(() => Type);}
        }

        private string _year;
        public string Year 
        {
            get { return _year; }
            set { _year = value; RaisePropertyChanged(() => Year);}
        }

        private string _docNumber;
        public string DocNumber 
        {
            get { return _docNumber; }
            set { _docNumber = value; RaisePropertyChanged(() => DocNumber);}
        }
    }
}