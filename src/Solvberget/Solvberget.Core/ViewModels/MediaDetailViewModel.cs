using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.DTOs;
using Solvberget.Core.Properties;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using System.Threading.Tasks;

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
			Title = title ?? "Detaljer";
            Availability = new DocumentAvailabilityDto {AvailableCount = 1};
            Availabilities = new DocumentAvailabilityViewModel[0];
            Load(docId);
        }

        private async void Load(string docId)
        {
            IsLoading = true;

            var review = _searchService.GetReview(docId);
            var rating = _searchService.GetRating(docId);

			ButtonEnabled = true; // obsolete now?
			IsReservable = true; // -"-
            
            var document = await _searchService.Get(docId);
            DocId = docId;
            Title = document.Title;
            SubTitle = document.SubTitle;
            ItemTitle = document.Title;
            Name = document.Title;
            Image = Resources.ServiceUrl + string.Format(Resources.ServiceUrl_MediaImage, docId);
            Year = (document.Year != 0) ? document.Year.ToString("####") : "Ukjent år";
            Type = document.Type;
            Author = document.MainContributor;
			IsReservedByUser = document.IsReserved.HasValue && document.IsReserved.Value;
			IsFavorite = document.IsFavorite.HasValue && document.IsFavorite.Value;

			ButtonText = GenerateButtonText();

            var classification = "";
            if (document.Type == "Book")
            {
                classification = ((BookDto) document).Classification;
            }
                

            Availabilities = (from a in document.Availability
                select new DocumentAvailabilityViewModel(_userService, this)
                {
                    AvailableCount = a.AvailableCount,
                    Branch = a.Branch,
                    Collection = a.Collection,
                    Department = a.Department,
                    Location = a.Location,
                    TotalCount = a.TotalCount,
                    DocId = docId,
                    ButtonText = ButtonText,
                    EstimatedAvailableDate = a.EstimatedAvailableDate,
                    IsReservable = IsReservable,
                    Classification = classification
                }).ToArray();

            foreach (var availabilityViewModel in Availabilities)
            {
				availabilityViewModel.IsReservable = true;
                availabilityViewModel.ButtonText = GenerateButtonText();
            }

            Availability = document.Availability.FirstOrDefault() ?? new DocumentAvailabilityDto {AvailableCount = 0, TotalCount = 0};
            RawDto = document;
            Language = document.Language;
            Languages = document.Languages;
            Publisher = document.Publisher;
            MainContributor = document.MainContributor;
            EstimatedAvailableDate = "Ukjent";
            if (document.Availability != null && Availability.EstimatedAvailableDate.HasValue)
            {
                EstimatedAvailableDate = Availability.EstimatedAvailableDate.Value.ToString("dd.MM.yyyy");
            }

            var reviewDto = await review;
            if (reviewDto.Success && !string.IsNullOrEmpty(reviewDto.Review))
            {
                Review = reviewDto.Review;
            }

            var ratingDto = await rating;
			if (ratingDto.Success && ratingDto.HasRating) Rating = ratingDto;

			IsLoading = false;
			NotifyViewModelReady();
        }

		public override void OnViewReady()
		{
			LoggedIn = _userAuthService.UserInfoRegistered();
		}

        private string GenerateButtonText()
        {
            if (IsReservedByUser)
            {
				return "Kanseller reservasjon";
            }

            return "Reserver";
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
            set { _subTitle = value; RaisePropertyChanged(() => SubTitle); }
        }

        private DocumentAvailabilityDto _availability;
        public DocumentAvailabilityDto Availability 
        {
            get { return _availability; }
            set 
            { 
                _availability = value; 
                RaisePropertyChanged(() => Availability);
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
            set
            {
                _type = value;
                RaisePropertyChanged(() => Type);
                RaisePropertyChanged(() => ReviewType);
            }
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

        private string[] _languages;
        public string[] Languages 
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

		public async Task AddFavorite()
        {
			if (!LoggedIn)
			{
				GotoLogin();
				return;
			}

			var result = await _userService.AddUserFavorite(DocId);
            
			if (result.Success)
			{
				IsFavorite = true;
			}
        }

		public async Task RemoveFavorite()
        {
			if (!LoggedIn)
			{
				GotoLogin();
				return;
			}

			var result = await _userService.RemoveUserFavorite(DocId);

			if(result.Success)
			{
            	IsFavorite = false;
			}
        }

		public void GotoLogin()
		{
			ShowViewModel<LoginViewModel>(new MvxBundle(new Dictionary<string,string>{{"navigateBackOnLogin", "true"}}));
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

        public string ReviewType 
        {
            get { return (Type == "Book" ? "BOKOMTALE" : "ANMELDELSE") ; }
        }

        private DocumentAvailabilityViewModel[] _availabilities;
        public DocumentAvailabilityViewModel[] Availabilities 
        {
            get { return _availabilities; }
            set { _availabilities = value; RaisePropertyChanged(() => Availabilities);}
        }

        public void RefreshButtons()
        {
            ButtonText = GenerateButtonText();
            foreach (var availability in Availabilities)
            {
                availability.IsReservable = IsReservable;
                availability.ButtonText = ButtonText;
            }
        }
    }
}