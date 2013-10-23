using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IDocumentService
    {
        Document Get(string docId);
    }
}