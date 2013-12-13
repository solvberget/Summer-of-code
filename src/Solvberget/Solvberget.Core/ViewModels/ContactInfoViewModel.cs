using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;

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
			Title = "Kontakt oss";

			var id = 0;

			InfoBoxes = (await _contactInfoService.GetContactInfo()).Select(ci =>{
				var vm = ContactInfoBoxViewModel.Map(ci);
				vm.Id = id++;
				return vm;
			}).ToList();

			IsLoading = false;
			NotifyViewModelReady();
        }

        

        private List<ContactInfoBoxViewModel> _infoBoxes;
        public List<ContactInfoBoxViewModel> InfoBoxes 
        {
            get { return _infoBoxes; }
            set { _infoBoxes = value; RaisePropertyChanged(() => InfoBoxes);}
        }


		private MvxCommand<ContactInfoBoxViewModel> _showDetailsCommand;
		public ICommand ShowDetailsCommand
		{
			get
			{
				return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<ContactInfoBoxViewModel>(ExecuteShowDetailsCommand));
			}
		}

		private void ExecuteShowDetailsCommand(ContactInfoBoxViewModel model)
		{
			ShowViewModel<ContactInfoBoxViewModel>(new {id = model.Id, title = model.Title});
		}
    }
}
