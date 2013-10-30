using Nancy;
using Solvberget.Domain.Blogs;

namespace Solvberget.Nancy.Modules
{
    public class BlogModule : NancyModule
    {
        public BlogModule(IBlogRepository repository) : base("/blogs")
        {
            Get["/"] = _ => repository.GetBlogs();

            Get["/{id}"] = args => repository.GetBlogWithEntries(args.id);
        }
    }
}