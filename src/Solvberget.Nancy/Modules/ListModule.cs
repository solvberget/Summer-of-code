using System.Globalization;
using System.Linq;

using Nancy;

using Solvberget.Domain.Abstract;

namespace Solvberget.Nancy.Modules
{
    public class ListModule : NancyModule
    {
        public ListModule(IListRepositoryStatic staticRepository, IListRepository dynamicRepository) : base("/lists")
        {
            Get["/static"] = _ =>
            {
                var resultStatic = staticRepository.GetLists(Request.Query.limit);
                var latestChange = staticRepository.GetTimestampForLatestChange();

                var timestamp = latestChange != null
                    ? latestChange.Value.Ticks.ToString(CultureInfo.InvariantCulture)
                    : "0";
                
                return new { Timestamp = timestamp, Lists = resultStatic };
            };

            Get["/dynamic"] = _ => dynamicRepository.GetLists(Request.Query.limit);

            Get["/static/last-modified"] = _ =>
            {
                var timestamp = staticRepository.GetTimestampForLatestChange();

                var response = timestamp != null 
                    ? timestamp.Value.Ticks.ToString(CultureInfo.InvariantCulture) 
                    : "0";

                return response;
            };

            Get["/combined"] = _ =>
            {
                int limit = Request.Query.limit;

                var resultStatic = staticRepository.GetLists(limit);
                var resultDynamic = dynamicRepository.GetLists(limit);
                var totalResult = resultStatic.Union(resultDynamic).OrderBy(x => x.Priority);
                var latestChange = staticRepository.GetTimestampForLatestChange();

                var timestamp = latestChange != null 
                    ? latestChange.Value.Ticks.ToString(CultureInfo.InvariantCulture) 
                    : "0";
                
                return new { TimestampForStatic = timestamp, Lists = totalResult };
            };
        }
    }
}