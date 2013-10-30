using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class SuggestionsService : ISuggestionsService
    {
        public async Task<List<LibraryList>> GetSuggestionsLists()
        {
            await TaskEx.Delay(200);
            throw new NotImplementedException();
        }

        public async Task<List<Document>> GetList(string id)
        {
            await TaskEx.Delay(200);
            throw new NotImplementedException();
        } 
    }
}
