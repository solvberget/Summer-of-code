using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<BlogDto>> GetBlogListing();
        Task<BlogWithPostsDto> GetBlogPostListing(long blogId);
        Task<BlogPostDto> GetBlogPost(string blogId, string postId);
    }
}