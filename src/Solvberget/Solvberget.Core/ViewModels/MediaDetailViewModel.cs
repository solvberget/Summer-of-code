using System.Xml.Linq;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Domain.DTO;

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
            Load(_docService.Get(docId));
        }

        private void Load(Document document)
        {
            Title = document.Title;
            Name = document.Title;
            Year = document.PublishedYear.ToString("0000");
            Type = document.DocType;
            Author = document.MainResponsible.ToString();
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
    }
}