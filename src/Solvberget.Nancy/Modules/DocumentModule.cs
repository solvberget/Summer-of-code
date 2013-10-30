using System;
using System.Linq;
using Nancy;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Nancy.Mapping;

namespace Solvberget.Nancy.Modules
{
    public class DocumentModule : NancyModule
    {
        public DocumentModule(IRepository documents, IImageRepository images, IRatingRepository ratings)
            : base("/documents")
        {
            Get["/{id}/thumbnail"] = args =>
            {
                string url = images.GetDocumentImage(args.id);
                return Response.AsRedirect(url);
            };

            Get["/{id}"] = args =>
            {
                Document document = documents.GetDocument(args.id, false);
                return Response.AsJson(DtoMaps.Map(document));
            };

            Get["/{id}/rating"] = args =>
            {
                DocumentRating rating = ratings.GetDocumentRating(args.id);
                return Response.AsJson(rating);
            };
        }

    }
}
