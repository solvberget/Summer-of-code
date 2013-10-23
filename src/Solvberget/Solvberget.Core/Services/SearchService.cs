using System.Collections.Generic;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    class SearchService : ISearchService
    {
        public IEnumerable<Document> Search(string query)
        {
            // TODO: Implement
            return new List<Document>();
        }
    }
}
