using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Utils;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Stubs
{
    class DocumentServiceTemporaryStub : IDocumentService
    {
        private readonly IBackgroundWorker _bgWorker;

        public DocumentServiceTemporaryStub(IBackgroundWorker bgWorker)
        {
            _bgWorker = bgWorker;
        }

        public async Task<Document> Get(string docId)
        {
            await TaskEx.Delay(2500); // Simulate some network latency
            return new Document()
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