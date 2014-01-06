using System;

namespace Solvberget.Core.DTOs
{
    public class NewsStoryDto : RequestReplyDto
    {
        public string Title { get; set; }
        public string Ingress { get; set; }
        public DateTime Published { get; set; }
        public Uri Link { get; set; }
        public string ImageUrl { get; set; }
    }
}
