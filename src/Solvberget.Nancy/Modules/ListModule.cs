using System;
using System.IO;
using System.Linq;
using Nancy;
using Nancy.LightningCache.Extensions;
using Nancy.Responses;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Documents.Images;
using Solvberget.Domain.Utils;
using Solvberget.Nancy.Mapping;
using Solvberget.Domain.Lists;

namespace Solvberget.Nancy.Modules
{
    public class ListModule : NancyModule
    {
        private readonly IRepository _documents;

        public ListModule(ILibraryListRepository lists, IRepository documents, IImageRepository images, IEnvironmentPathProvider pathProvider)
            : base("/lists")
        {
            _documents = documents;
            
            Get["/"] = _ =>
            {
                var results = lists.GetAll().Select(doc => DtoMaps.Map(doc));
                return Response.AsJson(results).AsCacheable(DateTime.Now.AddDays(1)); // fucks up CORS... ?
            };

            Get["/{id}"] = args =>
            {
                LibrarylistDto dto = DtoMaps.Map(lists.Get(args.id), _documents);
                return Response.AsJson(dto).AsCacheable(DateTime.Now.AddDays(1));
            };

            Get["/{id}/thumbnail"] = args =>
            {
                LibraryList list = lists.Get(args.id);

                foreach (var docNo in list.DocumentNumbers.Keys)
                {
                    var img = images.GetDocumentImage(docNo);

                    if (String.IsNullOrEmpty(img)) continue;

                    return Response.AsFile(Path.Combine(pathProvider.GetImageCachePath(), img)).AsCacheable(DateTime.Now.AddDays(1));
                }

                return TextResponse.NoBody; // todo: placeholder img
            };
        }
        
    }
}