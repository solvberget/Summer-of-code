using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class OpeningHoursViewModel : BaseViewModel
    {
        private readonly IOpeningHoursService _openingHoursService;

        public OpeningHoursViewModel(IOpeningHoursService openingHoursService)
        {
            _openingHoursService = openingHoursService;
        }

        public void Init()
        {
            Title = "Åpningstider";
            Load();
        }

        private List<OpeningHoursLocationViewModel> _locations;
        public List<OpeningHoursLocationViewModel> Locations 
        {
            get { return _locations; }
            set { _locations = value; RaisePropertyChanged(() => Locations);}
        }

        public async Task Load()
        {
            IsLoading = true;
            Locations = (await _openingHoursService.GetOpeningHours()).Select(oh => 
                new OpeningHoursLocationViewModel
                {
                    Hours = oh.Hours.ToDictionary(k => k.Title, v => v.Hours),
                    LocationName = oh.Title,
                    Phone = oh.Phone,
                    Title = oh.Title,
                    Url = oh.Url,
                    UrlText = oh.UrlText
                }).ToList();
            IsLoading = false;
        }

        private MvxCommand<OpeningHoursLocationViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<OpeningHoursLocationViewModel>(ExecuteShowDetailsCommand));
            }
        }

        private void ExecuteShowDetailsCommand(OpeningHoursLocationViewModel newsStory)
        {
            
        }
    }

    public class OpeningHoursLocationViewModel : BaseViewModel
    {
        private string _title;
        public new string Title 
        {
            get { return _title; }
            set 
            {
                _title = value;
                LocationName = value;
                RaisePropertyChanged(() => Title);
            }
        }


        private string _locationName;
        public string LocationName
        {
            get { return _locationName; }
            set
            {
                _locationName = value;
                RaisePropertyChanged(() => LocationName);
            }
        }

        private Dictionary<string, string> _hours;
        public Dictionary<string, string> Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                RaisePropertyChanged(() => Hours);
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                RaisePropertyChanged(() => Phone);
            }
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                RaisePropertyChanged(() => Url);
            }
        }

        private string _urlText;
        public string UrlText
        {
            get { return _urlText; }
            set
            {
                _urlText = value;
                RaisePropertyChanged(() => UrlText);
            }
        }
    }
}
