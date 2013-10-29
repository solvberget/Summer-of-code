using System.Collections.Generic;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract.V2
{
    public interface ILibraryListRepository
    {
        IEnumerable<LibraryList> GetAll();
        LibraryList Get(string id);
    }
}