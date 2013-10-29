using System;
using System.Collections.Generic;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPageLoansViewModel : BaseViewModel
    {
        public MyPageLoansViewModel(IUserInformationService service)
        {
            if (service == null) throw new ArgumentNullException("service");

            Loans = service.GetUserLoans("id");
        }

        private List<Loan> _loans;
        public List<Loan> Loans
        {
            get { return _loans; }
            set { _loans = value; RaisePropertyChanged(() => Loans); }
        } 
    }
}
