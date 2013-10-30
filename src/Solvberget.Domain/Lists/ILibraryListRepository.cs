using System.Collections.Generic;

namespace Solvberget.Domain.Lists
{
    public interface ILibraryListRepository
    {
        IEnumerable<LibraryList> GetAll();
        LibraryList Get(string id);
    }
}