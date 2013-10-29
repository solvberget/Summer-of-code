using System.Threading.Tasks;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services.Stubs
{
    class DocumentServiceTemporaryStub : IDocumentService
    {
        public async Task<Document> Get(string docId)
        {
            await TaskEx.Delay(2500); // Simulate some network latency
            return new Document
            {
                DocumentNumber = docId,
                Title = "Hello World Media",
                PublishedYear = 2003,
                Publisher = "Harris Media Information Publishing House",
                MainResponsible = "Harris Ford"
            };
        }
    }
}