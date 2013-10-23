using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    class SearchService : ISearchService
    {
        public async Task<IEnumerable<Document>>  Search(string query)
        {
            // TODO: Implement
            await TaskEx.Delay(5000);
            return new List<Document>();
        }
    }
}
