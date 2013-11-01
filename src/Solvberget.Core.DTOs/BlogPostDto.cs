using System;

namespace Solvberget.Core.DTOs
{
    public class BlogPostDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
    }
}
