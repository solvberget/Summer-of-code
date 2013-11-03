using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Nancy;
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
                return blogs.Select((bp, index) => new BlogDto
                {
                    Description = bp.Description,
                    Title = bp.Title,
                    Url = bp.Url,
                    Id = index
                });
            };

            Get["/{id}"] = args =>
            {
                IEnumerable<Blog> blogs = repository.GetBlogWithEntries(args.id);
                Blog blog = Enumerable.ElementAt(blogs, args.id);
                return blog.Entries.Select((bp, index) =>
                    new BlogPostOverviewDto
                    {
                        Author = bp.AuthorName,
                        Description = bp.Description,
                        Id = index,
                        Published = bp.PublishedDate,
                        Title = bp.Title
                    });
            };

            Get["/{id}/{postId}"] = args =>
            {
                Blog blog = repository.GetBlogWithEntries(args.id);
                BlogEntry correctEntry = Enumerable.ElementAt(blog.Entries, args.postId);
                return new BlogPostDto()
                {
                    Author = correctEntry.AuthorName,
                    Title = correctEntry.Title,
                    Content = correctEntry.Content,
                    BlogId = args.id,
                    Id = args.postId,
                    Published = correctEntry.PublishedDate
                };
            };
        }
    }
}