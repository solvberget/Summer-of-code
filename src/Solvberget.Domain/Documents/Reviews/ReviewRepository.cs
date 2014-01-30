using System.Web;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Integrations.BokBasen;

namespace Solvberget.Domain.Documents.Reviews
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly IRepository _documentRepository;
        private readonly BokbasenRepository _bokbasenRepository;

        public ReviewRepository(IRepository documentRepository)
        {
            _documentRepository = documentRepository;
            _bokbasenRepository = new BokbasenRepository(_documentRepository);

        }


        public string GetDocumentReview(string id)
        {
            var bokbasenBook = _bokbasenRepository.GetExternalBokbasenBook(id);
            string review = null;
            if (bokbasenBook != null)
            {
                review = bokbasenBook.Fsreview;
                //The text is saved in URL format, so we decode it
                review = HttpUtility.UrlDecode(review, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                        }
            return review;
        }


    }
}
