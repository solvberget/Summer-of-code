using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface ISuggestionsService
    {
        Task<List<LibraryList>> GetSuggestionsLists();
        Task<List<Document>> GetList(string id);
    }
}
