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
    }
}
