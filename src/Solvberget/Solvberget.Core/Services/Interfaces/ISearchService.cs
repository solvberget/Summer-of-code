using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.DTOs.Deprecated.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<DocumentDto>> Search(string query);
    }
}