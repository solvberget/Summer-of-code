using System;
using System.Resources;
using Solvberget.Core.DTOs;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Properties;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MediaDetailViewModel : BaseViewModel
    {
        private readonly ISearchService _searchService;

        public MediaDetailViewModel(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public void Init(string docId)
        {
            Load(docId);
        }

        private async void Load(string docId)
        {
            IsLoading = true;

            var document = await _searchService.Get(docId);
            Title = document.Title;
            ItemTitle = document.Title;
            Name = document.Title;
            Image = Resources.ServiceUrl + string.Format(Resources.ServiceUrl_MediaImage, docId);
            Year = document.Year.ToString("0000");
            Type = document.Type;
            TypeAndYear = String.Format("{0} ({1})", Type, Year);
            Author = document.MainContributor;
            Availability = document.Availability;

            IsLoading = false;
        }

        private DocumentAvailabilityDto _availability;
        public DocumentAvailabilityDto Availability 
        {
            get { return _availability; }
            set 
            { 
                _availability = value; 
                RaisePropertyChanged(() => Availability);
                RaisePropertyChanged(() => AvailabilitySummary);
            }
        }

        private string _availabilitySummary;
        public string AvailabilitySummary 
        {
            get
            {
                return string.Format("{0} av {1} tilgjengelig", Availability.AvailableCount, Availability.TotalCount);
            }
        }


        private string _image;
        public string Image 
        {
            get { return _image; }
            set { _image = value; RaisePropertyChanged(() => Image);}
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