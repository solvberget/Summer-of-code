using System.Collections.Generic;

namespace Solvberget.Domain.Lists
{
    public interface IListRepository
    {
        List<LibraryList> GetLists(int? limit = null);
    }
}