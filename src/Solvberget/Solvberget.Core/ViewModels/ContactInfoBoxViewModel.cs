using System.Collections.Generic;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class ContactInfoBoxViewModel : BaseViewModel
    {
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