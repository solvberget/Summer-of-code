using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solvberget.Core.DTOs
{
    public class BlogWithPostsDto : BlogDto
    {
        public BlogPostOverviewDto[] Posts { get; set; }
    }

    public class BlogPostOverviewDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
