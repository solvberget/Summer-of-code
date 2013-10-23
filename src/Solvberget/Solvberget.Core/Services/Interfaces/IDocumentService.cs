using System;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IDocumentService
    {
        void Get(string docId, Action<Document> callback);
    }
}