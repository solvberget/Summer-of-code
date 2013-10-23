using System;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Utils;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Stubs
{
    class DocumentServiceTemporaryStub : IDocumentService
    {
        private readonly IBackgroundWorker _bgWorker;

        public DocumentServiceTemporaryStub(IBackgroundWorker bgWorker)
        {
            _bgWorker = bgWorker;
        }

        public void Get(string docId, Action<Document> callback)
        {
            _bgWorker.Invoke(() =>
                callback(new Document()
                {
                    DocumentNumber = docId,
                    Title = "Hello World Media",
                    PublishedYear = 2003,
                    Publisher = "Harris Media Information Publishing House",
                    MainResponsible = "Harris Ford"
                })
            );
        }
    }
}