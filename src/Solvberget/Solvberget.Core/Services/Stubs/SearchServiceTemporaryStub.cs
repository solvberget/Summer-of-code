using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services.Stubs
{
    internal class SearchServiceTemporaryStub : ISearchService
    {
        public async Task<IEnumerable<DocumentDto>> Search(string query)
        {
            await TaskEx.Delay(2500); // Simulate some network latency
            return new List<DocumentDto>
            {
                new DocumentDto {Title = "Harry Potter", Type = "Book"},
                new DocumentDto {Title = "Harry Potter and the Prisoner from Azkaban", Type = "Film" },
                new DocumentDto {Title = "The Wall", Type ="Sheet Music" },
                new DocumentDto {Title = "The Wall", Type = "Cd"}
            };
        }
    }
}