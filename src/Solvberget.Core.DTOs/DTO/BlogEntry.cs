using System;

namespace Solvberget.Domain.DTO
{
    public class BlogEntry
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public string PublishedDateFormatted
        {
            get { return PublishedDate.Year == 1 ? null : PublishedDate.ToString("dd.MM.yyyy"); }
        }

        public DateTime UpdatedDate { get; set; }
        public string UpdatedDateFormatted
        {
            get { return UpdatedDate.Year == 1 ? null : UpdatedDate.ToString("dd.MM.yyyy"); }
        }
        public string AuthorName { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Description { get; set; }
    }
}
