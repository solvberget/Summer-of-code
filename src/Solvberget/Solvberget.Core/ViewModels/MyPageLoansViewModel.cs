using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPageLoansViewModel : BaseViewModel
    {
        private readonly IUserService _service;
        private readonly IUserAuthenticationDataService _userAuthenticationService;

        public MyPageLoansViewModel(IUserService service, IUserAuthenticationDataService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
            _service = service;
            Load();
        }

        private ObservableCollection<LoanViewModel> _loans;
        public ObservableCollection<LoanViewModel> Loans
        {
            get { return _loans; }
            set { _loans = value; RaisePropertyChanged(() => Loans); }
        }

        public async void Load()
        {
            IsLoading = true;

            var user = await _service.GetUserInformation(_userAuthenticationService.GetUserId());

            var loansList = user.Loans == null ? new List<LoanDto>() : user.Loans.ToList();

            Loans = new ObservableCollection<LoanViewModel>();

            foreach (LoanDto l in loansList)
            {
                Loans.Add(new LoanViewModel
                {
                    DocumentTitle = l.DocumentTitle,
                    DueDate = l.DueDate,
                    Material = l.Material,
                    SubLibrary = l.SubLibrary,
                    ButtonVisible = true
                });
            }

            if (Loans.Count == 0)
            {
                Loans.Add(new LoanViewModel
                {
                    DocumentTitle = "Ingen registrerte lån, ta deg en tur på biblioteket",
                    ButtonVisible = false

                });
            }
            IsLoading = false;
        }

        public void RemoveReservation(LoanViewModel loanViewModel)
        {
            Loans.Remove(loanViewModel);
        }

        public void AddReservation(LoanViewModel loanViewModel)
        {
            Loans.Add(loanViewModel);
        }
    }
}
