using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Solvberget.Core.DTOs;
using Solvberget.Core.Properties;
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
                var due = "";
                if (l.DueDate != null)
                {
                    due = l.DueDate.Value.ToString("dd.MM.yyyy");
                }

                Loans.Add(new LoanViewModel
                {
                    DocumentTitle = l.DocumentTitle,
                    DueDate = due,
                    Material = l.Material,
                    SubLibrary = l.SubLibrary,
                    ButtonVisible = true,
                    Image = Resources.ServiceUrl + string.Format(Resources.ServiceUrl_MediaImage, l.DocumentNumber),
                    Parent = this,
                    DocumentNumber = l.DocumentNumber
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

        public void ExpandLoan(string documentNumber)
        {
            _service.ExpandLoan(documentNumber);
        }
    }
}
