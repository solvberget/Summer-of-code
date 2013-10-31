using System;
using System.Collections.Generic;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPageFinesViewModel : BaseViewModel
    {
        public MyPageFinesViewModel(IUserService service)
        {
            if (service == null) throw new ArgumentNullException("service");

            //Fines = service.GetUserFines("id");
        }

        private List<Fine> _fines;
        public List<Fine> Fines
        {
            get { return _fines; }
            set { _fines = value; RaisePropertyChanged(() => Fines); }
        } 
    }
}
