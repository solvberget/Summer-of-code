using System;
using Cirrious.CrossCore;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.ViewModels
{
    public class MyPageViewModel : BaseViewModel
    {
        public MyPageViewModel()
        {
            MyPageLoansViewModel = new MyPageLoansViewModel();
            MyPagePersonaliaViewModel = new MyPagePersonaliaViewModel();
            MyPageReservationsViewModel = new MyPageReservationsViewModel();
            MyPageFinesViewModel = new MyPageFinesViewModel();
            MyPageMessagesViewModel = new MyPageMessagesViewModel();
            MyPageFavoritesViewModel = new MyPageFavoritesViewModel();
        }

        private MyPageLoansViewModel myPageLoansViewModel;
        public MyPageLoansViewModel MyPageLoansViewModel
        {
            get { return myPageLoansViewModel; }
            set { myPageLoansViewModel = value; RaisePropertyChanged(() => MyPageLoansViewModel); }
        }

        private MyPagePersonaliaViewModel myPagePersonaliaViewModel;
        public MyPagePersonaliaViewModel MyPagePersonaliaViewModel
        {
            get { return myPagePersonaliaViewModel; }
            set { myPagePersonaliaViewModel = value; RaisePropertyChanged(() => MyPagePersonaliaViewModel); }
        }

        private MyPageReservationsViewModel myPageReservationsViewModel;
        public MyPageReservationsViewModel MyPageReservationsViewModel
        {
            get { return myPageReservationsViewModel; }
            set { myPageReservationsViewModel = value; RaisePropertyChanged(() => MyPagePersonaliaViewModel); }
        }

        private MyPageFinesViewModel myPageFinesViewModel;
        public MyPageFinesViewModel MyPageFinesViewModel
        {
            get { return myPageFinesViewModel; }
            set { myPageFinesViewModel = value; RaisePropertyChanged(() => MyPageFinesViewModel); }
        }

        private MyPageMessagesViewModel myPageMessagesViewModel;
        public MyPageMessagesViewModel MyPageMessagesViewModel
        {
            get { return myPageMessagesViewModel; }
            set { myPageMessagesViewModel = value; RaisePropertyChanged(() => MyPageMessagesViewModel); }
        }
        private MyPageFavoritesViewModel myPageFavoritesViewModel;
        public MyPageFavoritesViewModel MyPageFavoritesViewModel
        {
            get { return myPageFavoritesViewModel; }
            set { myPageFavoritesViewModel = value; RaisePropertyChanged(() => MyPageFavoritesViewModel); }
        }
    }
}
