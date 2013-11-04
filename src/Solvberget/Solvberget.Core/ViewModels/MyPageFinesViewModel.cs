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

        private List<FineDto> _fines;
        public List<FineDto> Fines
        {
            get { return _fines; }
            set { _fines = value; RaisePropertyChanged(() => Fines); }
        }

        public async void Load()
        {
            var user = await _service.GetUserInformation(_service.GetUserId());

            Fines = user.Fines == null ? new List<FineDto>() : user.Fines.ToList();

            if (Fines.Count == 0)
            {
                Fines = new List<FineDto>
                {
                    new FineDto
                    {
                        Description =
                            "Du har ingen gebyrer! Det kan du for eksempel få hvis du leverer noe for sent eller mister noe"
                    }
                };
            }
            else
            {
                for (var i = 0; i < Fines.Count; i++)
                {
                    if (Fines.ElementAt(i).Status.Equals("Cancelled"))
                    {
                        Fines.Remove(Fines.ElementAt(i));
                        i--;
                    }
                }
            }
            IsLoading = false;
        }
    }
}
