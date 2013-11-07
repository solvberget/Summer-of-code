using System;
using System.Collections.Generic;
using System.Linq;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPageFinesViewModel : BaseViewModel
    {
        private readonly IUserService _service;

        public MyPageFinesViewModel(IUserService service)
        {
            _service = service;
            Load();
        }

        private List<FineViewModel> _fines;
        public List<FineViewModel> Fines
        {
            get { return _fines; }
            set { _fines = value; RaisePropertyChanged(() => Fines); }
        }

        public async void Load()
        {
            var user = await _service.GetUserInformation(_service.GetUserId());

            var finesDtos = user.Fines == null ? new List<FineDto>() : user.Fines.ToList();

            Fines = new List<FineViewModel>();

            foreach (FineDto f in finesDtos)
            {
                Fines.Add(new FineViewModel
                {
                    Description = f.Description,
                    DocumentTitle = f.DocumentTitle,
                    Sum = Convert.ToInt32(f.Sum) + ",-",
                    Status = f.Status
                });
            }

            if (Fines.Count != 0)
            {
                for (var i = 0; i < Fines.Count; i++)
                {
                    if (Fines.ElementAt(i).Status.Equals("Cancelled") || Fines.ElementAt(i).Status.Equals("Paid"))
                    {
                        Fines.Remove(Fines.ElementAt(i));
                        i--;
                    }
                }
            }

            if (Fines.Count == 0)
            {
                Fines.Add(new FineViewModel
                    {
                        Description =
                            "Du har ingen gebyrer! Det kan du for eksempel få hvis du leverer noe for sent eller mister noe"
                    });
            }
        }
    }
}
