using System;
using System.Linq;
using Nancy;
using Nancy.Responses;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Documents.Images;
using Solvberget.Nancy.Mapping;
using Solvberget.Domain.Lists;

namespace Solvberget.Nancy.Modules
{
    public class ListModule : NancyModule
    {
        private readonly IRepository _documents;

        public ListModule(ILibraryListRepository lists, IRepository documents, IImageRepository images)
            : base("/lists")
        {
            _documents = documents;
            
            Get["/"] = _ =>
            {
                var results = lists.GetAll().Select(doc => DtoMaps.Map(doc));
                return Response.AsJson(results); //.AsCacheable(DateTime.Now.AddSeconds(30)); // fucks up CORS...
            };

            Get["/{id}"] = args =>
            {
                LibrarylistDto dto = DtoMaps.Map(lists.Get(args.id), _documents);
                return Response.AsJson(dto);
            };

            Get["/{id}/thumbnail"] = args =>
            {
                LibraryList list = lists.Get(args.id);

                foreach (var docNo in list.DocumentNumbers.Keys)
                {
                    var url = images.GetDocumentImage(docNo);

                    if (String.IsNullOrEmpty(url)) continue;

                    return Response.AsRedirect(url);
                }

                return TextResponse.NoBody;
            };
        }
        
    }
}