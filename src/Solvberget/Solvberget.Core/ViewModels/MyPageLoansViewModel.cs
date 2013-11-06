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

        public MyPageLoansViewModel(IUserService service)
        {
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

            var user = await _service.GetUserInformation(_service.GetUserId());

            var loansList = user.Loans == null ? new List<LoanDto>() : user.Loans.ToList();

            Loans = new ObservableCollection<LoanViewModel>();

            foreach (LoanDto l in loansList)
            {
                Loans.Add(new LoanViewModel
                {
                    DocumentTitle = l.DocumentTitle,
                    DueDate = l.DueDate,
                    Material = l.Material,
                    SubLibrary = l.SubLibrary
                });
            }

            //if (Loans.Count == 0)
            //{
            //    Loans = new List<LoanDto>
            //    {
            //        new LoanDto
            //        {
            //            DocumentTitle = "Ingen regisrterte lån! Ta en tur til biblioteket og finn deg noe!"
            //        }
            //    };
            //}
            IsLoading = false;
        }
    }
}
