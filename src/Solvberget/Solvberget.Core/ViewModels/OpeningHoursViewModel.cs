using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class OpeningHoursViewModel : BaseViewModel
    {
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
            await TaskEx.Delay(750);
            Locations = new List<OpeningHoursLocationViewModel>
            {
                new OpeningHoursLocationViewModel
                {
                    LocationName = "Kulturhusets Foajé",
                    Phone = "51 51 07 00",
                    Hours = new Dictionary<string, string> {{"Mandag - Torsdag", "10.00-22.00"}, {"Fredag og Lørdag", "10.00-23.00"},{"Søndag", "12.00-22.00"}}
                },
                new OpeningHoursLocationViewModel
                {
                    LocationName = "SF Kino Stavanger, Billettluke",
                    Phone = "51 51 07 00",
                    Hours = new Dictionary<string, string> {{"Mandag - Torsdag", "11.30 - 21.30"}, {"Fredag", "11.30 - 22.30"}, {"Lørdag", "12.00 - 22.30"}, {"Søndag", "12.00 - 21.30"}}
                },
                new OpeningHoursLocationViewModel
                {
                    LocationName = "Choco Boco",
                    Phone = "51 86 33 08",
                    Hours = new Dictionary<string, string> {{"Mandag - Torsdag", "10.00 - 22.00"}, {"Fredag", "10.00 - 24.00"}, {"Lørdag", "11.00 - 24.00"}, {"Søndag", "12.00 - 21.30"}}
                }
            };
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
