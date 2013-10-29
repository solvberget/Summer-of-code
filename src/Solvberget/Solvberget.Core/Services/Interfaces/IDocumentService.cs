using System.Threading.Tasks;
using Solvberget.Core.DTOs.Deprecated.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<Document> Get(string docId);
    }
}