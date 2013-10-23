using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    public interface IDocumentService
    {
        Document Get(string docId);
    }

    class DocumentServiceTemporaryStub : IDocumentService
    {
        public Document Get(string docId)
        {
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

    class DocumentService : IDocumentService
    {
        public Document Get(string docId)
        {
            // TODO: Implement
            return new Document();
        }
    }
}
