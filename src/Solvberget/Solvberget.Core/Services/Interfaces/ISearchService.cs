using System;
using System.Collections.Generic;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface ISearchService
    {
        void  Search(string query, Action<IEnumerable<Document>> callback);
    }
}