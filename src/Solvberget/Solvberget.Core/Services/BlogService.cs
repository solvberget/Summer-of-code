using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Properties;

namespace Solvberget.Core.Services
{
    public class BlogService : IBlogService
    {
        private readonly DtoDownloader _downloader;

        public BlogService(DtoDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<List<BlogDto>> GetBlogListing()
        {
            var response = await _downloader.DownloadList<BlogDto>(Resources.ServiceUrl + Resources.ServiceUrl_BlogListing);
            return response.Results;
        }

        public async Task<BlogWithPostsDto> GetBlogPostListing(long blogId)
        {
            return await _downloader.Download<BlogWithPostsDto>(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_BlogDetails, blogId));
        }

        public async Task<BlogPostDto> GetBlogPost(string blogId, string postId)
        {
            return await _downloader.Download<BlogPostDto>(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_BlogPost, blogId, postId));
        }
    }
}