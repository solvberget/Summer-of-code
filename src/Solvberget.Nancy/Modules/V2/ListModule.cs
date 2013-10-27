using System;
using System.Globalization;
using System.Linq;
using Autofac.Features.Indexed;
using Nancy;
using Nancy.LightningCache.Extensions;
using Nancy.Responses;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Nancy.Modules.V2
{
    public class ListModule : NancyModule
    {
        private readonly IRepository _documents;
        private readonly IImageRepository _images;

        public ListModule(IIndex<ListRepository, IListRepository> repositories, IRepository documents, IImageRepository images)
            : base("/v2/lists")
        {
            _documents = documents;
            _images = images;
            var staticRepository = (IListRepositoryStatic)repositories[ListRepository.Static];
            var dynamicRepository = repositories[ListRepository.Dynamic];

            Get["/"] = _ =>
            {
                int? limit = Request.Query.limit.HasValue ? Request.Query.limit : null;

                var resultStatic = staticRepository.GetLists(limit);
                var resultDynamic = dynamicRepository.GetLists(limit);

                IOrderedEnumerable<LibraryList> totalResult = resultStatic.Union(resultDynamic).OrderBy(x => x.Priority);

                var results = totalResult.Select(MapToDto);
                
                return Response.AsJson(results); //.AsCacheable(DateTime.Now.AddSeconds(30)); // fucks up CORS...
            };
        }
        
        private dynamic MapToDto(LibraryList list)
        {
            return new
            {
                list.Id,
                list.Name,
                Documents = list.Documents.Count > 0 ? list.Documents.Select(MapToDto) : list.DocumentNumbers.Keys.Select(dn => MapToDto(FetchDocument(dn)))
            };
        }

        private dynamic MapToDto(Document document)
        {
            return new
            {
                document.Title,
                document.SubTitle
            };
        }

        private Document FetchDocument(string documentNumber)
        {
            return _documents.GetDocument(documentNumber, true);
        }

        private static string GetTimestamp(DateTime? latestChange)
        {
            return latestChange.HasValue ? latestChange.Value.Ticks.ToString(CultureInfo.InvariantCulture) : "0";
        }
    }
}