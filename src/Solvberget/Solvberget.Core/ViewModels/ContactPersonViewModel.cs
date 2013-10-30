using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class ContactPersonViewModel : BaseViewModel
    {
        private string _name;
        public string Name 
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name);}
        }

        private string _position;
        public string Position 
        {
            get { return _position; }
            set { _position = value; RaisePropertyChanged(() => Position);}
        }

        private string _email;
        public string Email 
        {
            get { return _email; }
            set { _email = value; RaisePropertyChanged(() => Email);}
        }

        private string _phone;
        public string Phone 
        {
            get { return _phone; }
            set { _phone = value; RaisePropertyChanged(() => Phone);}
        }
    }
}