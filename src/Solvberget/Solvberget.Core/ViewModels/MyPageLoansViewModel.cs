using System;
using Cirrious.CrossCore;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPageLoansViewModel : BaseViewModel
    {
        public MyPageLoansViewModel()
        {
            var service = Mvx.Resolve<IUserInformationService>();
            if (service == null) throw new ArgumentNullException("service");

            var user = service.GetUserInformation("id");

        }
    }
}
