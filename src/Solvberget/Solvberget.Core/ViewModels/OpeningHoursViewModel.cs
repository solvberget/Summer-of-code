using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Core.DTOs;
using System;

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
			var id = 0;
            IsLoading = true;
            Locations = (await _openingHoursService.GetOpeningHours()).Select(oh => 
				new OpeningHoursLocationViewModel(_openingHoursService)
                {
                    Hours = oh.Hours.ToDictionary(k => k.Title, v => v.Hours),
                    LocationName = oh.Title,
                    Phone = oh.Phone,
                    Title = oh.Title,
                    Url = oh.Url,
					UrlText = oh.UrlText,
					Id = id++
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

		private void ExecuteShowDetailsCommand(OpeningHoursLocationViewModel model)
        {
			this.ShowViewModel<OpeningHoursLocationViewModel>(new {id = model.Id, title = model.Title});
        }
    }

    public class OpeningHoursLocationViewModel : BaseViewModel
	{
		IOpeningHoursService _service;

		public OpeningHoursLocationViewModel(IOpeningHoursService openingHoursService)
		{
			_service = openingHoursService;

		}

		public void Init(string id, string title)
		{
			Title = title;
			Load(Int32.Parse(id));
		}

		private async Task Load(int id)
		{
			IsLoading = true;
			var ohs = await _service.GetOpeningHours();
				
			var oh = ohs.Skip(id).FirstOrDefault();

		    Hours = oh.Hours.ToDictionary(k => k.Title, v => v.Hours);

			LocationName = oh.Title;
			Phone = oh.Phone;
			Title = oh.Title;
			Url = oh.Url;
			UrlText = oh.UrlText;
			
          	IsLoading = false;
			NotifyViewModelReady();
		}

        private string _title;
        public new string Title 
        {
            get { return _title; }
            set 
            {
                _title = value;
				base.Title = value;
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
