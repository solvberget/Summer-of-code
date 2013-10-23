using System;
using System.Threading.Tasks;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<Document> Get(string docId);
    }
}