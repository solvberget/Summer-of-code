using System;
using System.Collections.Generic;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    class SearchService : ISearchService
    {
        public void Search(string query, Action<IEnumerable<Document>> callback)
        {
            // TODO: Implement
            callback(new List<Document>());
        }
    }
}
