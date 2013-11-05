using System;
using System.IO;
using System.Linq;
using Nancy;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Documents;
using Solvberget.Domain.Documents.Reviews;
using Solvberget.Domain.Utils;
using Solvberget.Nancy.Mapping;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Documents.Images;
using Solvberget.Domain.Documents.Ratings;

namespace Solvberget.Nancy.Modules
{
    public class DocumentModule : NancyModule
    {
        public DocumentModule(IRepository documents, IImageRepository images, IRatingRepository ratings, IReviewRepository reviews, IEnvironmentPathProvider pathProvider)
            : base("/documents")
        {
            Get["/{id}/thumbnail"] = args =>
            {
                string img = images.GetDocumentImage(args.id);

                if (String.IsNullOrEmpty(img))
                {
                    return HttpStatusCode.NotFound;
                }

                return Response.AsFile(Path.Combine(pathProvider.GetImageCachePath(), img));
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

            Get["/{id}/review"] = args =>
            {
                string review = reviews.GetDocumentReview(args.id);
                return new DocumentReviewDto{ Review = review, Url = "" };
            };

            Get["/search"] = _ =>
            {
                string query = Request.Query.query.HasValue ? Request.Query.query : null;

                if (null == query) throw new InvalidOperationException("Ingenting å søke etter.");

                return documents.Search(query).Select(DtoMaps.Map).ToArray();
            };
        }

    }
}
