using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac.Features.Indexed;
using Nancy;
using Nancy.LightningCache.Extensions;
using Nancy.Responses;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Abstract.V2;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation.V2;

namespace Solvberget.Nancy.Modules.V2
{
    public class ListModule : NancyModule
    {
        private readonly IRepository _documents;

        public ListModule(ILibraryListRepository lists, IRepository documents, IImageRepository images)
            : base("/v2/lists")
        {
            _documents = documents;
            
            Get["/"] = _ =>
            {
                int? limit = Request.Query.limit.HasValue ? Request.Query.limit : null;
                
                var results = lists.GetAll().Select(doc => MapLibraryListToDto(doc));
                
                return Response.AsJson(results); //.AsCacheable(DateTime.Now.AddSeconds(30)); // fucks up CORS...
            };

            Get["/{id}"] = args =>
            {
                LibrarylistDto dto = MapLibraryListToDto(lists.Get(args.id), true);
                return Response.AsJson(dto);
            };
        }
        
        private dynamic MapLibraryListToDto(LibraryList list, bool includeDocuments = true)
        {
            if (includeDocuments)
            {
                return new LibrarylistDto()
                {
                    Id = list.Id,
                    Name = list.Name,
                    Documents = list.Documents.Count > 0
                        ? list.Documents.Select(MapDocumentToDto).ToList()
                        : list.DocumentNumbers.Keys.Select(dn => MapDocumentToDto(FetchDocument(dn))).ToList()
                };
            }

            return new
            {
                list.Id,
                list.Name
            };
        }

        private DocumentDto MapDocumentToDto(Document document)
        {
            return new DocumentDto
            {
                Id = document.DocumentNumber,
                Title = document.Title,
                SubTitle = document.SubTitle,
                Type = document.DocType
            };
        }

        private Document FetchDocument(string documentNumber)
        {
            return _documents.GetDocument(documentNumber, true);
        }
    }

    public class LibrarylistDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DocumentDto> Documents { get; set; }
    }

    public class DocumentDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Type { get; set; }
    }
}