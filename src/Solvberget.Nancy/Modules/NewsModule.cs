using System.Collections.Generic;
using Nancy;
using System.Linq;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Info;

namespace Solvberget.Nancy.Modules
{
    public class NewsModule : NancyModule
    {
        public NewsModule(INewsRepository newsRepository) : base("/news")
        {
            Get["/"] = args =>
            {
                IList<NewsItem> results = newsRepository.GetNewsItems(Request.Query.limit);
                return results.Select(ni => new NewsStoryDto
                {
                    Title = ni.Title,
                    Ingress = ni.Description,
                    Link = ni.Link,
                    Published = ni.PublishedDateAsDateTime.DateTime
                });
            };
        }
    }
}
