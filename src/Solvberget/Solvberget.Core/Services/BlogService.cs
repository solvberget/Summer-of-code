using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class BlogService : IBlogService
    {
        public Task<List<BlogDto>> GetBlogListing()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<BlogPostOverviewDto>> GetBlogPostListing(long blogId)
        {
            throw new System.NotImplementedException();
        }

        public Task<BlogPostDto> GetBlogPost(string blogId, string postId)
        {
            throw new System.NotImplementedException();
        }
    }
}