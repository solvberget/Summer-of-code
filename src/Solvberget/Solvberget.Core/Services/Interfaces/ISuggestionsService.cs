using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Nancy.Modules.V2;

namespace Solvberget.Core.Services.Interfaces
{
    public interface ISuggestionsService
    {
        Task<List<LibrarylistDto>> GetSuggestionsLists();
        Task<LibrarylistDto> GetList(string id);
    }
}
