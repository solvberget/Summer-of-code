﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Properties;

namespace Solvberget.Core.Services
{
    public class BlogService : IBlogService
    {
        private readonly IStringDownloader _downloader;

        public BlogService(IStringDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<List<BlogDto>> GetBlogListing()
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_BlogListing);
                return JsonConvert.DeserializeObject<List<BlogDto>>(response);
            }
            catch (Exception)
            {

                return new List<BlogDto>
                {
                    new BlogDto
                    {
                        Title = "Ikke funnet",
                        Description = "Kunne desverre ikke finne noen blogger"
                    }
                };
            }

        }

        public async Task<BlogWithPostsDto> GetBlogPostListing(long blogId)
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_BlogDetails, blogId));
                return JsonConvert.DeserializeObject<BlogWithPostsDto>(response);
            }
            catch (Exception)
            {
                return new BlogWithPostsDto
                {
                    Posts = new[]
                    {
                        new BlogPostOverviewDto
                        {
                            Title = "Ikke funnet",
                            Description = "Kunne desverre ikke finne noen bloggposter"
                        }
                    }
                };
            }
        }

        public async Task<BlogPostDto> GetBlogPost(string blogId, string postId)
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_BlogPost, blogId, postId));
                return JsonConvert.DeserializeObject<BlogPostDto>(response);
            }
            catch (Exception)
            {
                return new BlogPostDto
                {
                    Title = "Ikke funnet",
                    Content = "Kunne desverre ikke finne bloggposten"
                };
            }
        }
    }
}