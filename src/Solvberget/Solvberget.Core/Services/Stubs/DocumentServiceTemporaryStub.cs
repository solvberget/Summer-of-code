using Solvberget.Core.Services.Interfaces;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Stubs
{
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
}