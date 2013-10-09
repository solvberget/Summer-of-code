using System.Collections.Generic;

using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IListRepository
    {
        List<LibraryList> GetLists(int? limit = null);
    }
}