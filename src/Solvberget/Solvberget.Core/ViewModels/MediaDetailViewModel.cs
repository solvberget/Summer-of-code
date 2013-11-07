using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Init(string title, string docId)
        {
            Title = title;
            Availability = new DocumentAvailabilityDto {AvailableCount = 1};
            Load(docId);
        }

        private async void Load(string docId)
        {
            IsLoading = true;

            var document = await _searchService.Get(docId);
            Title = document.Title;
            SubTitle = document.SubTitle;
            ItemTitle = document.Title;
            Name = document.Title;
            Image = Resources.ServiceUrl + string.Format(Resources.ServiceUrl_MediaImage, docId);
            Year = document.Year.ToString("0000");
            Type = document.Type;
            TypeAndYear = String.Format("{0} ({1})", Type, Year);
            Author = document.MainContributor;
            Availability = document.Availability;
            RawDto = document;
            Language = document.Language;
            Languages = document.Languages.ToList();
            Publisher = document.Publisher;
            MainContributor = document.MainContributor;
            EstimatedAvailableDate = "Ukjent";
            if (document.Availability != null && document.Availability.EstimatedAvailableDate.HasValue)
            {
                EstimatedAvailableDate = document.Availability.EstimatedAvailableDate.Value.ToString("dd-MM-yyyy");
            }

            IsLoading = false;
        }

        private DocumentDto _rawDto;
        public DocumentDto RawDto 
        {
            get { return _rawDto; }
            set { _rawDto = value; RaisePropertyChanged(() => RawDto);}
        }

        private string _subTitle;
        public string SubTitle
        {
            get { return _subTitle; }
            set
            {
                _subTitle = value;
                RaisePropertyChanged(() => SubTitle);
            }
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

        private string _estimatedAvailableDate;
        public string EstimatedAvailableDate 
        {
            get { return _estimatedAvailableDate; }
            set { _estimatedAvailableDate = value; RaisePropertyChanged(() => EstimatedAvailableDate);}
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

        private string _language;
        public string Language 
        {
            get { return _language; }
            set { _language = value; RaisePropertyChanged(() => Language);}
        }

        private List<string> _languages;
        public List<string> Languages 
        {
            get { return _languages; }
            set { _languages = value; RaisePropertyChanged(() => Languages);}
        }


        private string _mainContributor;
        public string MainContributor 
        {
            get { return _mainContributor; }
            set { _mainContributor = value; RaisePropertyChanged(() => MainContributor);}
        }

        private string _publisher;
        public string Publisher 
        {
            get { return _publisher; }
            set { _publisher = value; RaisePropertyChanged(() => Publisher);}
        }
    }
}