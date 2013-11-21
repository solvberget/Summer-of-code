using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class EventViewModel : BaseViewModel
    {
        private string _description;
        public string Description 
        {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(() => Description);}
        }

        private string _price;
        public string Price 
        {
            get { return _price; }
            set { 
				_price = value; 
				RaisePropertyChanged(() => Price);
				RaisePropertyChanged(() => TimeAndPlaceSummary);
			}
        }

        private string _time;
        public string Time 
        {
            get { return _time; }
            set { 
				_time = value; 
				RaisePropertyChanged(() => Time);
				RaisePropertyChanged(() => TimeAndPlaceSummary);
			}
        }

        private string _location;
        public string Location 
        {
            get { return _location; }
            set { 
				_location = value; 
				RaisePropertyChanged(() => Location);
				RaisePropertyChanged(() => TimeAndPlaceSummary);
			}
        }

        private string _imageUrl;
        public string ImageUrl 
        {
            get { return _imageUrl; }
            set { _imageUrl = value; RaisePropertyChanged(() => ImageUrl);}
        }

        private string _url;
        public string Url 
        {
            get { return _url; }
            set { _url = value; RaisePropertyChanged(() => Url);}
        }

        private string _date;
        public string Date 
        {
            get { return _date; }
            set { 
				_date = value; 
				RaisePropertyChanged(() => Date);
				RaisePropertyChanged(() => TimeAndPlaceSummary);
			}
        }

		public string TimeAndPlaceSummary 
		{
			get { return string.Format("{0} {1} - {2}", Date, Time, Price); }
		}
    }
}
