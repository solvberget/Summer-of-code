using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services.Stubs
{
    class DocumentServiceTemporaryStub : IDocumentService
    {
        public async Task<DocumentDto> Get(string docId)
        {
            await TaskEx.Delay(1000); // Simulate some network latency
            return new DocumentDto
            {
                Id = docId,
                Title = "Hello World Media",
                Year = 2003,
                Publisher = "Harris Media Information Publishing House",
                MainContributor = "Harris Ford"
            };
        }
    }
}