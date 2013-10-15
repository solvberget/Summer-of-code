using Nancy;
using Nancy.ModelBinding;

using Solvberget.Domain.Abstract;

namespace Solvberget.Nancy.Modules
{
    public class DocumentModule : NancyModule
    {
        public DocumentModule(
            IRepository documentRepository,
            IImageRepository imageRepository,
            IRatingRepository ratingRepository,
            IReviewRepository reviewRepository) : base("/documents")
        {
            Get["/search"] = _ => documentRepository.Search(Request.Query.q);

            Get["/{id}"] = args => documentRepository.GetDocument(args.id, Request.Query.light ?? false);
            
            Get["/{id}/review"] = args => reviewRepository.GetDocumentReview(args.id);
            
            Post["/{id}/reservation"] = args =>
            {
                var model = this.Bind();
                return documentRepository.RequestReservation(args.id, model.userId, model.branch);
            };

            Delete["/{id}/reservation"] = args =>
            {
                var model = this.Bind();
                return documentRepository.CancelReservation(args.id, model.itemSequence, model.cancellationSequence);
            };
            
            Post["/{id}/renew"] = args =>
            {
                var model = this.Bind();
                return documentRepository.RequestRenewalOfLoan(args.id,
                    model.itemSequence,
                    model.barcode,
                    model.userId);
            };

            Get["/{id}/rating"] = args => ratingRepository.GetDocumentRating(args.id);

            Get["/{id}/image"] = args =>
            {
                bool isThumbnail = Request.Query.thumb ?? false;
                if (isThumbnail)
                {
                    return imageRepository.GetDocumentThumbnailImage(args.id, Request.Query.size);
                }

                return imageRepository.GetDocumentImage(args.id);
            };
        }
    }
}
