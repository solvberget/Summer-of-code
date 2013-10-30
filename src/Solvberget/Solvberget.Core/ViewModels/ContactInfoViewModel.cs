using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class ContactInfoViewModel : BaseViewModel
    {
        private readonly IContactInformationService _contactInfoService;

        public ContactInfoViewModel(IContactInformationService contactInfoService)
        {
            _contactInfoService = contactInfoService;
        }

        public void Init()
        {
            Load();
        }

        private async Task Load()
        {
            IsLoading = true;

            InfoBoxes = (await _contactInfoService.GetContactInfo()).Select(ci =>
                new ContactInfoBoxViewModel
                {
                    Address = ci.Address,
                    ContactPersons = new List<ContactPersonViewModel>(),
                    Email = ci.Email,
                    Fax = ci.Fax,
                    GenericFields = new List<string>(),
                    Phone = ci.Phone,
                    Title = ci.Title,
                    VisitingAddress = ci.VisitingAddress,
                }).ToList();

            IsLoading = false;
        }

        private List<ContactInfoBoxViewModel> _infoBoxes;
        public List<ContactInfoBoxViewModel> InfoBoxes 
        {
            get { return _infoBoxes; }
            set { _infoBoxes = value; RaisePropertyChanged(() => InfoBoxes);}
        }
    }
}
