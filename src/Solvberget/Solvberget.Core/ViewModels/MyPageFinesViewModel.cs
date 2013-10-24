using System;
using System.Collections.Generic;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.ViewModels
{
    public class MyPageFinesViewModel : BaseViewModel
    {
        public MyPageFinesViewModel(IUserInformationService service)
        {
            if (service == null) throw new ArgumentNullException("service");

            Fines = service.GetUserFines("id");
        }

        private List<Fine> _fines;
        public List<Fine> Fines
        {
            get { return _fines; }
            set { _fines = value; RaisePropertyChanged(() => Fines); }
        } 
    }
}
