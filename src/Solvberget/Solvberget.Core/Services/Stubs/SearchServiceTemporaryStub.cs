using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Utils;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Stubs
{
    internal class SearchServiceTemporaryStub : ISearchService
    {
        private readonly IBackgroundWorker _bgWorker;

        public SearchServiceTemporaryStub(IBackgroundWorker bgWorker)
        {
            _bgWorker = bgWorker;
        }

        public async Task<IEnumerable<Document>> Search(string query)
        {
            await TaskEx.Delay(2500); // Simulate some network latency
            return new List<Document>()
            {
                new Document() {Title = "Harry Potter", PublishedYear = 2008},
                new Film()
                {
                    Title = "Harry Potter and the Prisoner from Azkaban",
                    NorwegianTitle = "Harry Potter og fangen fra Azkaban",
                },
                new Cd()
                {
                    Title = "The Wall",
                    ArtistOrComposer = new Person() {Name = "Pink Floyd"},
                    PublishedYear = 1983
                },
                new SheetMusic()
                {
                    Title = "The Wall",
                    Composer = new Person() {Name = "David Gilmour"},
                    PublishedYear = 1983
                }
            };
        }
    }
}