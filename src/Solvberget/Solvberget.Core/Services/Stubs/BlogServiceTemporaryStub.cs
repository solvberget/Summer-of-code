using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services.Stubs
{
    public class BlogServiceTemporaryStub : IBlogService
    {
        public async Task<List<BlogDto>> GetBlogListing()
        {
            await TaskEx.Delay(200);
            return new List<BlogDto>
            {
                new BlogDto { Title = "Hello World-Bloggen", Id = 1, Url = "http://solvberget.no", Description = "En blogg om alt som er morsomt"},
                new BlogDto { Title = "Hello World-Bloggen", Id = 2, Url = "http://solvberget.no", Description = "Diverse rart"},
                new BlogDto { Title = "Hello World-Bloggen", Id = 3, Url = "http://solvberget.no", Description = "Skalviseda... siste bloggen her."},
            };
        }

        public async Task<List<BlogPostOverviewDto>> GetBlogPostListing(long blogId)
        {
            await TaskEx.Delay(200);
            return new List<BlogPostOverviewDto>
            {
                new BlogPostOverviewDto { Author = "Ola Normann", Description = "En spennende post", Title = "Spennende post", Published = DateTime.Now, Id = 1},
                new BlogPostOverviewDto { Author = "Kari Normann", Description = "En helt ok post", Title = "Byråkratiets meritter", Published = DateTime.Now, Id = 2},
                new BlogPostOverviewDto { Author = "Per Svenske", Description = "En litt kjedelig post", Title = "Heisan", Published = DateTime.Now, Id = 2},
            };
        }

        public async Task<BlogPostDto> GetBlogPost(string blogId, string postId)
        {
            await TaskEx.Delay(200);
            return new BlogPostDto
            {
                Author = "Ola Normann", Content = "En lang og spennende rekke med masse innhold her skal vi se da hvordan dette blir. Regner med det kanskje er noe html og noe slikt også? Skal vi se.... \n\n Jajaja. Nytt avsnitt her.\n\nEnnå et nytt avsnitt.", Title = "Spennende post", Published = DateTime.Now, Id = "1"
            };
        }
    }
}