using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Stubs
{
    public class SuggestionsServiceTemporaryStub : ISuggestionsService
    {
        public async Task<List<LibraryList>> GetSuggestionsList()
        {
            await TaskEx.Delay(1200);

            return new List<LibraryList>
            {
                new LibraryList
                {
                    Name = "Harry Potter er kul",
                    Documents = new List<Document>
                    {
                        new Document {Title = "Harry Potter", PublishedYear = 2008},
                        new Film
                        {
                            Title = "Harry Potter and the Prisoner from Azkaban",
                            NorwegianTitle = "Harry Potter og fangen fra Azkaban",
                        },
                        new Cd
                        {
                            Title = "The Wall",
                            ArtistOrComposer = new Person {Name = "Pink Floyd"},
                            PublishedYear = 1983
                        },
                        new SheetMusic
                        {
                            Title = "The Wall",
                            Composer = new Person {Name = "David Gilmour"},
                            PublishedYear = 1983
                        }
                    }
                },
                new LibraryList
                {
                    Name = "Det finnes andre morsomme ting..",
                    Documents = new List<Document>
                    {
                        new Document {Title = "Harry Potter", PublishedYear = 2008},
                        new Film
                        {
                            Title = "Harry Potter and the Prisoner from Azkaban",
                            NorwegianTitle = "Harry Potter og fangen fra Azkaban",
                        },
                        new Cd
                        {
                            Title = "The Wall",
                            ArtistOrComposer = new Person {Name = "Pink Floyd"},
                            PublishedYear = 1983
                        },
                        new SheetMusic
                        {
                            Title = "The Wall",
                            Composer = new Person {Name = "David Gilmour"},
                            PublishedYear = 1983
                        }                        
                    }
                }
            };
        }
    }
}
