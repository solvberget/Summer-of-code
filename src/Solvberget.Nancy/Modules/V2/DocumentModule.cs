using System;
using Nancy;
using Nancy.LightningCache.Extensions;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Nancy.Modules.V2
{
    public class DocumentModule : NancyModule
    {
        public DocumentModule(IRepository documents, IImageRepository images)
            : base("/v2/documents")
        {
            Get["/{id}/thumbnail"] = args =>
            {
                string url = images.GetDocumentThumbnailImage(args.id, "60");
                return Response.AsRedirect(url);
            };

            Get["/{id}/details"] = args =>
            {
                Document document = documents.GetDocument(args.id, true);
                return Response.AsJson(document);
            };
        }
    }
}