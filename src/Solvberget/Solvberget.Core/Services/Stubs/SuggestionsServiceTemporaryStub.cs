using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services.Stubs
{
    public class SuggestionsServiceTemporaryStub : ISuggestionsService
    {

        public async Task<LibrarylistDto> GetList(string id)
        {
            await TaskEx.Delay(2000);

            return new LibrarylistDto
            {
                Documents = new List<DocumentDto>
                {
                    new DocumentDto {Title = "Harry Potter", Year = "2008"},
                    new DocumentDto
                    {
                        Title = "Harry Potter and the Prisoner from Azkaban",
                        SubTitle = "Harry Potter og fangen fra Azkaban",
                    },
                    new DocumentDto
                    {
                        Title = "The Wall",
                        SubTitle = "Pink Floyd",
                        Year = "1983"
                    },
                    new DocumentDto
                    {
                        Title = "The Wall",
                        SubTitle = "David Gilmour",
                        Year = "1983"
                    }
                }
            };
        }

        public async Task<List<LibrarylistDto>> GetSuggestionsLists()
        {
            await TaskEx.Delay(200);

            return new List<LibrarylistDto>
            {
                new LibrarylistDto
                {
                    Name = "Harry Potter er kul",
                    Documents = new List<DocumentDto>
                    {
                        new DocumentDto {Title = "Harry Potter", Year = "2008"},
                        new DocumentDto
                        {
                            Title = "Harry Potter and the Prisoner from Azkaban",
                            SubTitle = "Harry Potter og fangen fra Azkaban",
                        },
                        new DocumentDto
                        {
                            Title = "The Wall",
                            SubTitle = "Pink Floyd",
                            Year = "1983"
                        },
                        new DocumentDto
                        {
                            Title = "The Wall",
                            SubTitle = "David Gilmour",
                            Year = "1983"
                        }
                    }
                },
                new LibrarylistDto
                {
                    Name = "Harry Potter er kul",
                    Documents = new List<DocumentDto>
                    {
                        new DocumentDto {Title = "Harry Potter", Year = "2008"},
                        new DocumentDto
                        {
                            Title = "Harry Potter and the Prisoner from Azkaban",
                            SubTitle = "Harry Potter og fangen fra Azkaban",
                        },
                        new DocumentDto
                        {
                            Title = "The Wall",
                            SubTitle = "Pink Floyd",
                            Year = "1983"
                        },
                        new DocumentDto
                        {
                            Title = "The Wall",
                            SubTitle = "David Gilmour",
                            Year = "1983"
                        }
                    }
                }
            };
        }
    }
}
