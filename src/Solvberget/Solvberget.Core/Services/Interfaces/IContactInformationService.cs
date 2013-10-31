using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IContactInformationService
    {
        Task<IList<ContactInfoDto>> GetContactInfo();
    }
}
