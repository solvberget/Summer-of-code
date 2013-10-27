using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac.Features.Indexed;
using Nancy;
using Nancy.ViewEngines;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

namespace Solvberget.Nancy.Modules
{
    public class ListModule : NancyModule
    {
        public ListModule(LibraryListDynamicRepository dynamicRepository, LibraryListXmlRepository staticRepository)
            : base("/lists")
        {
            Get["/static"] = _ =>
            {
                int? limit = Request.Query.limit.HasValue ? Request.Query.limit : null;

                var resultStatic = staticRepository.GetLists(limit);
                var latestChange = staticRepository.GetTimestampForLatestChange();

                var timestamp = GetTimestamp(latestChange);

                return new { Timestamp = timestamp, Lists = resultStatic };
            };

            Get["/static/last-modified"] = _ => GetTimestamp(staticRepository.GetTimestampForLatestChange());

            Get["/dynamic"] = _ => dynamicRepository.GetLists(Request.Query.limit);

            Get["/combined"] = _ =>
            {
                int? limit = Request.Query.limit.HasValue ? Request.Query.limit : null;

                var resultStatic = staticRepository.GetLists(limit);
                var resultDynamic = dynamicRepository.GetLists(limit);

                var totalResult = resultStatic.Union(resultDynamic).OrderBy(x => x.Priority);

                var timestamp = GetTimestamp(staticRepository.GetTimestampForLatestChange());
                
                return new { TimestampForStatic = timestamp, Lists = totalResult };
            };
        }

        private static string GetTimestamp(DateTime? latestChange)
        {
            return latestChange.HasValue ? latestChange.Value.Ticks.ToString(CultureInfo.InvariantCulture) : "0";
        }
    }
}