using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Nancy;
using Nancy.LightningCache.Extensions;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Blogs;
using System.Linq;

namespace Solvberget.Nancy.Modules
{
    public class BlogModule : NancyModule
    {
        public BlogModule(IBlogRepository repository) : base("/blogs")
        {
            Get["/"] = _ =>
            {
                IEnumerable<Blog> blogs = repository.GetBlogs();
                return Response.AsJson(blogs.Select((bp, index) => new BlogDto
                {
                    Description = bp.Description,
                    Title = bp.Title,
                    Url = bp.Url,
                    Id = index
                })).AsCacheable(DateTime.Now.AddHours(12));
            };

            Get["/{id}"] = args =>
            {
                Blog blog = repository.GetBlogWithEntries(args.id);
                
                var posts = blog.Entries.Select((bp, index) =>
                    new BlogPostOverviewDto
                    {
                        Author = bp.AuthorName,
                        Description = bp.Description,
                        Id = index,
                        Published = bp.PublishedDate,
                        Title = bp.Title,
                        ThumbnailUrl = bp.ThumbnailUrl,
                        Url = bp.Url
                    });

                return Response.AsJson(new BlogWithPostsDto
                {
                    Id = args.id,
                    Description = blog.Description,
                    Title = blog.Title,
                    Url = blog.Url,
                    Posts = posts.ToArray()
                }).AsCacheable(DateTime.Now.AddHours(3));
            };

            Get["/{id}/{postId}"] = args =>
            {
                Blog blog = repository.GetBlogWithEntries(args.id);
                BlogEntry correctEntry = Enumerable.ElementAt(blog.Entries, args.postId);
                return Response.AsJson(new BlogPostDto()
                {
                    Author = correctEntry.AuthorName,
                    Title = correctEntry.Title,
                    Content = correctEntry.Content,
                    BlogId = args.id,
                    Id = args.postId,
                    Published = correctEntry.PublishedDate
                }).AsCacheable(DateTime.Now.AddHours(3));
            };
        }
    }
}