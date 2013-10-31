using System.Threading.Tasks;
using Solvberget.Core.DTOs;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentDto> Get(string docId);
    }
}