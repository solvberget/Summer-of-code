using System.Collections.Generic;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface ISearchService
    {
        IEnumerable<Document> Search(string query);
    }
}