using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class HomeScreenViewModel : BaseViewModel
    {

        public void Init()
        {
            Title = "Startside";
            MyPage = "m";
            Lists = "h";
            OpeningHours = "å";
            Event = "a";
            Blogs = "e";
            News = "n";
            Contact = "c";
            Search = "s";
        }

        private string _myPage; 
        public string MyPage
        {
            get { return _myPage; }
            set { _myPage = value; RaisePropertyChanged(() => MyPage); }
        }

        private string _event;
        public string Event 
        {
            get { return _event; }
            set { _event = value; RaisePropertyChanged(() => Event);}
        } 

        private string _blogs;
        public string Blogs 
        {
            get { return _blogs; }
            set { _blogs = value; RaisePropertyChanged(() => Blogs);}
        } 
        private string _news;
        public string News 
        {
            get { return _news; }
            set { _news = value; RaisePropertyChanged(() => News);}
        } 
        private string _lists;
        public string Lists 
        {
            get { return _lists; }
            set { _lists = value; RaisePropertyChanged(() => Lists);}
        } 

        private string _openingHours;
        public string OpeningHours 
        {
            get { return _openingHours; }
            set { _openingHours = value; RaisePropertyChanged(() => OpeningHours);}
        } 

        private string _contact;
        public string Contact 
        {
            get { return _contact; }
            set { _contact = value; RaisePropertyChanged(() => Contact);}
        } 

        private string _search;
        public string Search 
        {
            get { return _search; }
            set { _search = value; RaisePropertyChanged(() => Search);}
        } 
    }
}
