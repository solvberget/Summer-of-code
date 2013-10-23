using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    class DocumentService : IDocumentService
    {
        public async Task<Document> Get(string docId)
        {
            // TODO: Implement
            await TaskEx.Delay(5000);
            return new Document();
        }
    }
}
