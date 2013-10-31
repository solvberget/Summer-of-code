using System;
using System.Linq;
using Nancy;
﻿using Solvberget.Domain.Documents;
using Solvberget.Domain.Documents.Reviews;
using Solvberget.Nancy.Mapping;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Documents.Images;
using Solvberget.Domain.Documents.Ratings;

namespace Solvberget.Nancy.Modules
{
    public class DocumentModule : NancyModule
    {
        public DocumentModule(IRepository documents, IImageRepository images, IRatingRepository ratings, IReviewRepository reviews)
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

            Get["/{id}/review"] = args =>
            {
                string review = reviews.GetDocumentReview(args.id);
                return review;
            };
        }

    }
}
