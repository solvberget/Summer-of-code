using FakeItEasy;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    class ReviewRepositoryTest
    {
        private readonly IReviewRepository _reviewRepository;
        private const string ImageCache = "";

        public ReviewRepositoryTest()
        {
            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetImageCachePath()).Returns(ImageCache);

            _reviewRepository = new ReviewRepository(new AlephRepository(fake));
        }

        [Fact]
        public void TestGetDocumentReview()
        {
            // 000605680 - Harry Potter og dødstalismanene del 2, size: default (150)
            const string harryPotterMovie = "000605680";
            var hpMovieReview = _reviewRepository.GetDocumentReview(harryPotterMovie);
            Assert.Null(hpMovieReview);

            const string harryPotterBook = "000610109";
            var hpBookReview = _reviewRepository.GetDocumentReview(harryPotterBook);
            Assert.Null(hpMovieReview);
            Assert.True(hpBookReview.Equals("Harry Potter tror han er en helt vanlig 11 år gammel" +
                                            " gutt til han blir reddet av ei ugle, tatt med til skolen" +
                                            " for hekser og trollmenn og vinner en duell som kunne fått" +
                                            " en dødelig utgang. Harry Potter er nemlig en trollmann!" +
                                            " Dette er den første boken om Harry Potter."));
        }
    }
}
