namespace Solvberget.Core.DTOs
{
    public class BlogDto : RequestReplyDto
    {
        public string Title { get; set; }
        public long Id { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
