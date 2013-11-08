using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;

namespace Solvberget.Core.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<DocumentDto>> Search(string query);
        Task<DocumentDto> Get(string docId);
        Task<DocumentRatingDto> GetRating(string docId);
        Task<DocumentReviewDto> GetReview(string docId);
    }
}