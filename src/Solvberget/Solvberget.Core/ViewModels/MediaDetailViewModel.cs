using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MediaDetailViewModel : BaseViewModel
    {
        private readonly ISearchService _searchService;
        private readonly IUserService _userService;
        private readonly IUserAuthenticationDataService _userAuthService;

        public MediaDetailViewModel(ISearchService searchService, IUserService userService, IUserAuthenticationDataService userAuthService)
        {
            _userAuthService = userAuthService;
            _userService = userService;
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

            var review = _searchService.GetReview(docId);
            var rating = _searchService.GetRating(docId);

            LoggedIn = _userAuthService.UserInfoRegistered();

            var docsReservedByUser = await _userService.GetUserReserverdDocuments();

            IsReservedByUser = docsReservedByUser.Contains(docId);
            ButtonEnabled = !IsReservedByUser && LoggedIn;
            IsFavorite = await _userService.IsFavorite(docId);

            if (!LoggedIn)
            {
                ButtonText = "Logg inn for å reservere";
                IsReservable = false;
            } 
            else if (IsReservedByUser)
            {
                ButtonText = "Reservert";
                IsReservable = false;
            }
            else
            {
                ButtonText = "Reserver";
                IsReservable = true;
            }

            var document = await _searchService.Get(docId);
            DocId = docId;
            Title = document.Title;
            SubTitle = document.SubTitle;
            ItemTitle = document.Title;
            Name = document.Title;
            Image = Resources.ServiceUrl + string.Format(Resources.ServiceUrl_MediaImage, docId);
            Year = document.Year.ToString("0000");
            Type = document.Type;
            Author = document.MainContributor;
            Availability = document.Availability ?? new DocumentAvailabilityDto {AvailableCount = 0, TotalCount = 0};
            RawDto = document;
            Language = document.Language;
            Languages = document.Languages.ToList();
            Publisher = document.Publisher;
            MainContributor = document.MainContributor;
            EstimatedAvailableDate = "Ukjent";
            if (document.Availability != null && document.Availability.EstimatedAvailableDate.HasValue)
            {
                EstimatedAvailableDate = document.Availability.EstimatedAvailableDate.Value.ToString("dd.MM.yyyy");
            }

            var reviewDto = await review;
            if (reviewDto != null && !string.IsNullOrEmpty(reviewDto.Review))
            {
                Review = reviewDto.Review;
            }

            Rating = await rating;

            IsLoading = false;
        }

        private MvxCommand<MediaDetailViewModel> _placeHoldRequestCommand;
        public ICommand PlaceHoldRequestCommand
        {
            get
            {
                return _placeHoldRequestCommand ??
                       (_placeHoldRequestCommand = new MvxCommand<MediaDetailViewModel>(ExecutePlaceHoldRequestCommand));
            }
        }

        private void ExecutePlaceHoldRequestCommand(MediaDetailViewModel media)
        {
            var response = _userService.AddReservation(DocId);
            Load(DocId);
        }

        private bool _loggedIn;
        public bool LoggedIn
        {
            get { return _loggedIn; }
            set { _loggedIn = value; RaisePropertyChanged(() => LoggedIn); }
        }

        private string _docId;
        public string DocId
        {
            get { return _docId; }
            set { _docId = value; RaisePropertyChanged(() => DocId); }
        }

        private DocumentRatingDto _rating;
        public DocumentRatingDto Rating 
        {
            get { return _rating; }
            set { _rating = value; RaisePropertyChanged(() => Rating);}
        }

        private string _review = "";
        public string Review 
        {
            get { return _review; }
            set { _review = value; RaisePropertyChanged(() => Review);}
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
                if (Availability != null)
                {
                    return string.Format("{0} av {1} tilgjengelig", Availability.AvailableCount, Availability.TotalCount);  
                }
                return "Tilgjengelighet er ikke kjent";
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

        private bool _isReservedByUser;
        public bool IsReservedByUser
        {
            get { return _isReservedByUser; }
            set { _isReservedByUser = value; RaisePropertyChanged(() => IsReservedByUser); }
        }

        private string _buttonText;
        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; RaisePropertyChanged(() => ButtonText); }
        }

        private bool _buttonEnabled;
        public bool ButtonEnabled
        {
            get { return _buttonEnabled; }
            set { _buttonEnabled = value; RaisePropertyChanged(() => ButtonEnabled); }
        }

        public void AddFavorite()
        {
            _userService.AddUserFavorite(DocId);
            IsFavorite = true;
        }

        public void RemoveFavorite()
        {
            _userService.RemoveUserFavorite(DocId);
            IsFavorite = false;
        }

        private bool _isReservable;
        public bool IsReservable
        {
            get { return _isReservable; }
            set { _isReservable = value; RaisePropertyChanged(() => IsReservable); }
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                _isFavorite = value; 
                RaisePropertyChanged(() => IsFavorite); }
        }
    }
}