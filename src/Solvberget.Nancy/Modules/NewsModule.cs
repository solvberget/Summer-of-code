using Nancy;

using Solvberget.Domain.Abstract;

namespace Solvberget.Nancy.Modules
{
    public class NewsModule : NancyModule
    {
        public NewsModule(INewsRepository newsRepository) : base("/news")
        {
            Get["/"] = args => newsRepository.GetNewsItems(Request.Query.limit);
        }
    }
}
