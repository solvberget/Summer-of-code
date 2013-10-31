using System;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MediaDetailViewModel : BaseViewModel
    {
        private readonly IDocumentService _docService;

        public MediaDetailViewModel(IDocumentService docService)
        {
            _docService = docService;
        }

        public void Init(string docId)
        {
            Load(docId);
        }

        private async void Load(string docId)
        {
            IsLoading = true;

            var document = await _docService.Get(docId);
            Title = document.Title;
            ItemTitle = document.Title;
            Name = document.Title;
            Year = "0000";
            Type = document.Type;
            TypeAndYear = String.Format("{0} ({1})", Type, Year);
            Author = document.MainContributor;

            IsLoading = false;
        }

        private string _name;
        public string Name 
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name);}
        }

        private string _year;
        public string Year 
        {
            get { return _year; }
            set { _year = value; RaisePropertyChanged(() => Year);}
        }

        private string _type;
        public string Type 
        {
            get { return _type; }
            set { _type = value; RaisePropertyChanged(() => Type);}
        }

        private string _author;
        public string Author 
        {
            get { return _author; }
            set { _author = value; RaisePropertyChanged(() => Author);}
        }

        private string _itemTitle;
        public string ItemTitle 
        {
            get { return _itemTitle; }
            set { _itemTitle = value; RaisePropertyChanged(() => ItemTitle);}
        }

        private string _typeAndYear;
        public string TypeAndYear 
        {
            get { return _typeAndYear; }
            set { _typeAndYear = value; RaisePropertyChanged(() => TypeAndYear);}
        }

    }
}