using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.ViewModels;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IOpeningHoursService
    {
        Task<IList<OpeningHoursDto>> GetOpeningHours();
    }
}
