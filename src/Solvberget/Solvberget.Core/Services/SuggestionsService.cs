using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    public class SuggestionsService : ISuggestionsService
    {
        public async Task<List<LibraryList>> GetSuggestionsList()
        {
            await TaskEx.Delay(200);
            throw new NotImplementedException();
        }
    }
}
