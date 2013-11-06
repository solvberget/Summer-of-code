using System.Collections.Generic;
using Nancy;
using System.Linq;
using Nancy.Security;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Info;

namespace Solvberget.Nancy.Modules
{
    public class NewsModule : NancyModule
    {
        public NewsModule(INewsRepository newsRepository) : base("/news")
        {
            this.RequiresAuthentication();

            Get["/"] = args =>
            {
                int limit = Request.Query.limit.HasValue ? Request.Query.limit : 10;

                IList<NewsItem> results = newsRepository.GetNewsItems(limit);
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
