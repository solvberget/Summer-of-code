using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    public interface ISearchService
    {
        IEnumerable<Document> Search(string query);
    }

    class SearchServiceTemporaryStub : ISearchService
    {
        public IEnumerable<Document> Search(string query)
        {
            return new List<Document>()
            {
                new Document() { Title = "Harry Potter", },
                new Film() {Title = "Harry Potter and the Prisoner from Azkaban", NorwegianTitle = "Harry Potter og fangen fra Azkaban"},
                new Cd() {Title = "The Wall", ArtistOrComposer = new Person(){Name = "Pink Floyd"}},
                new SheetMusic() {Title ="The Wall", Composer = new Person(){Name="David Gilmour"}}
            };
        }
    }

    class SearchService : ISearchService
    {
        public IEnumerable<Document> Search(string query)
        {
            // TODO: Implement
            return new List<Document>();
        }
    }
}
