using System;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    class DocumentService : IDocumentService
    {
        public void Get(string docId, Action<Document> callback)
        {
            // TODO: Implement
            callback(new Document());
        }
    }
}
