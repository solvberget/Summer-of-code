using System.Collections.Generic;

namespace Solvberget.Domain.Blogs
{
    public interface IBlogRepository
    {
        List<Blog> GetBlogs();
        Blog GetBlogWithEntries(int blogId);
    }
}