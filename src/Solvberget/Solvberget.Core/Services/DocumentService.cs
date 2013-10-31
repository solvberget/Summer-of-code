using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    class DocumentService : IDocumentService
    {
        public async Task<DocumentDto> Get(string docId)
        {
            // TODO: Implement
            await TaskEx.Delay(5000);
            return new DocumentDto();
        }
    }
}
