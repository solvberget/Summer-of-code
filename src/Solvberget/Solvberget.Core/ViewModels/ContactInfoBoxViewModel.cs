using System.Collections.Generic;
using Solvberget.Core.ViewModels.Base;
using System;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using System.Linq;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.ViewModels
{
    public class ContactInfoBoxViewModel : BaseViewModel
	{
		private readonly IContactInformationService _contactInfoService;

		public ContactInfoBoxViewModel(IContactInformationService service = null)
		{
			_contactInfoService = service;
		}

		public static ContactInfoBoxViewModel Map(ContactInfoDto ci)
		{
			var vm = new ContactInfoBoxViewModel();
			vm.MapFrom(ci);

			return vm;
		}

		private void MapFrom(ContactInfoDto ci)
		{
			Address = ci.Address;
			ContactPersons = MapContactPersons(ci.ContactPersons).ToList();
			Email = ci.Email;
			Fax = ci.Fax;
			GenericFields = ci.GenericFields.NullSafeToList();
			Phone = ci.Phone;
			Title = ci.Title;
			VisitingAddress = ci.VisitingAddress;
		}

		private IEnumerable<ContactPersonViewModel> MapContactPersons(IEnumerable<ContactPersonDto> contactPersons)
		{
			if (contactPersons == null)
				return Enumerable.Empty<ContactPersonViewModel>();

			return contactPersons.Select(cp => new ContactPersonViewModel
				{
					Email = cp.Email,
					Name = cp.Name,
					Phone = cp.Phone,
					Position = cp.Position
				});
		}

		public void Init(string id, string title)
		{
			Title = title;
			Load(Int32.Parse(id));
		}

		private async Task Load(int id)
		{
			IsLoading = true;

			var dto = await _contactInfoService.GetContactInfo();

			MapFrom(dto.Skip(id).First());

			IsLoading = false;
			NotifyViewModelReady();
		}

        private string _phone;
        public string Phone 
        {
            get { return _phone; }
            set { _phone = value; RaisePropertyChanged(() => Phone);}
        }
    
        private string _fax;
        public string Fax 
        {
            get { return _fax; }
            set { _fax = value; RaisePropertyChanged(() => Fax);}
        }
    
        private string _email;
        public string Email 
        {
            get { return _email; }
            set { _email = value; RaisePropertyChanged(() => Email);}
        }

        private string _address;
        public string Address 
        {
            get { return _address; }
            set { _address = value; RaisePropertyChanged(() => Address);}
        }

        private string _visitingAddress;
        public string VisitingAddress 
        {
            get { return _visitingAddress; }
            set { _visitingAddress = value; RaisePropertyChanged(() => VisitingAddress);}
        }

        private List<ContactPersonViewModel> _contactPersons;
        public List<ContactPersonViewModel> ContactPersons 
        {
            get { return _contactPersons; }
            set { _contactPersons = value; RaisePropertyChanged(() => ContactPersons);}
        }

        private List<string> _genericFields;
        public List<string> GenericFields 
        {
            get { return _genericFields; }
            set { _genericFields = value; RaisePropertyChanged(() => GenericFields);}
        }
    }
}