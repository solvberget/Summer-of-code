using System;
using System.Linq;
using Nancy;
using Nancy.Responses;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Abstract.V2;
using Solvberget.Domain.DTO;

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
                int? limit = Request.Query.limit.HasValue ? Request.Query.limit : null;
                
                var results = lists.GetAll().Select(doc => MapLibraryListToDto(doc));
                
                return Response.AsJson(results); //.AsCacheable(DateTime.Now.AddSeconds(30)); // fucks up CORS...
            };

            Get["/{id}"] = args =>
            {
                LibrarylistDto dto = MapLibraryListToDto(lists.Get(args.id), true);
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
        
        private dynamic MapLibraryListToDto(LibraryList list, bool includeDocuments = false)
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
}