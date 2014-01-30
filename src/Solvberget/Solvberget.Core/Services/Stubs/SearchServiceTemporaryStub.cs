using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services.Stubs
{
    internal class SearchServiceTemporaryStub : ISearchService
    {
        public async Task<IEnumerable<DocumentDto>> Search(string query)
        {
            await TaskEx.Delay(2500); // Simulate some network latency
            return new List<DocumentDto>
            {
                new DocumentDto {Title = "Harry Potter", Type = "Book"},
                new DocumentDto {Title = "Harry Potter and the Prisoner from Azkaban", Type = "Film" },
                new DocumentDto {Title = "The Wall", Type ="Sheet Music" },
                new DocumentDto {Title = "The Wall", Type = "Cd"}
            };
        }

        public async Task<DocumentDto> Get(string docId)
        {
            await TaskEx.Delay(200);
            return new DocumentDto
            {
                Title = "Hello World"
            };
        }

        public async Task<DocumentRatingDto> GetRating(string docId)
        {
            return new DocumentRatingDto{MaxScore = 10.0, Score = 10.0, Source = "IMDB", SourceUrl = "imdb.com"};
        }

        public async Task<DocumentReviewDto> GetReview(string docId)
        {
            return new DocumentReviewDto
            {
                Review = "Dette er en anmeldelse av en film eller en bok eller et eller anent",
                Url = ""
            };
        }
    }
}