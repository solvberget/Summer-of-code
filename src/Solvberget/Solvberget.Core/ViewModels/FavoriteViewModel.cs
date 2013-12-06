using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class FavoriteViewModel : BaseViewModel
    {

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        private int _year;
        public int Year
        {
            get { return _year; }
            set { _year = value; RaisePropertyChanged(() => Year); }
        }

		public string PresentableTypeWithYear {
			get
			{
				return string.Format("{0} ({1})", PresentableType, Year);
			}
		}

		public string PresentableType
		{
			get
			{
				return Utils.ConvertMediaTypeToNiceString(Type);
			}
		}

        private string _image;
        public string Image 
        {
            get { return _image; }
            set { _image = value; RaisePropertyChanged(() => Image);}
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; RaisePropertyChanged(() => Type); }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set { _author = value; RaisePropertyChanged(() => Author); }
        }

        private string _itemTitle;
        public string ItemTitle
        {
            get { return _itemTitle; }
            set { _itemTitle = value; RaisePropertyChanged(() => ItemTitle); }
        }

        private string _typeAndYear;
        public string TypeAndYear
        {
            get { return _typeAndYear; }
            set { _typeAndYear = value; RaisePropertyChanged(() => TypeAndYear); }
        }

        private bool _buttonVisible;
        public bool ButtonVisible
        {
            get { return _buttonVisible; }
            set { _buttonVisible = value; RaisePropertyChanged(() => ButtonVisible); }
        }

        private MyPageFavoritesViewModel _parent;
        public MyPageFavoritesViewModel Parent
        {
            get { return _parent; }
            set { _parent = value; RaisePropertyChanged(() => Parent); }
        }

        private string _documentNumber;
        public string DocumentNumber
        {
            get { return _documentNumber; }
            set { _documentNumber = value; RaisePropertyChanged(() => DocumentNumber); }
        }

        

        private MvxCommand<FavoriteViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<FavoriteViewModel>(ExecuteShowDetailsCommand));
            }
        }

        private void ExecuteShowDetailsCommand(FavoriteViewModel favorite)
        {
            Parent.RemoveFavorite(DocumentNumber, this);

            if (Parent.Favorites.Count == 0)
            {
                Parent.AddFavorite(new FavoriteViewModel
                {
                    Name = "Du har ingen registrerte favoritter",
                    ButtonVisible = false

                });
            }
        }
    }
}
