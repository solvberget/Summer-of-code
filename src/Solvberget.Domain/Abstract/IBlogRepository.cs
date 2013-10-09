using System.Collections.Generic;

using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IBlogRepository
    {
        List<Blog> GetBlogs();

        Blog GetBlogWithEntries(int blogId);
    }
}