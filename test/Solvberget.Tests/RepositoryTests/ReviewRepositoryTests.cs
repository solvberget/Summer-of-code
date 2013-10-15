using System;
using System.IO;

using FakeItEasy;

using NUnit.Framework;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    class ReviewRepositoryTest
    {

        private IReviewRepository _reviewRepository;
        private const string ImageCache = "";
        

        [SetUp]
        public void InitRepository()
        {
            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetImageCachePath()).Returns(ImageCache);

            _reviewRepository = new ReviewRepository(new AlephRepository(fake));
        }

        [Test]
        public void TestGetDocumentReview()
        {
            // 000605680 - Harry Potter og dødstalismanene del 2, size: default (150)
            const string harryPotterMovie = "000605680";
            var hpMovieReview = _reviewRepository.GetDocumentReview(harryPotterMovie);
            Assert.IsNullOrEmpty(hpMovieReview);
            

            const string harryPotterBook = "000610109";
            var hpBookReview = _reviewRepository.GetDocumentReview(harryPotterBook);
            Assert.IsNullOrEmpty(hpMovieReview);
            Assert.True(hpBookReview.Equals("Harry Potter tror han er en helt vanlig 11 år gammel" +
                                            " gutt til han blir reddet av ei ugle, tatt med til skolen" +
                                            " for hekser og trollmenn og vinner en duell som kunne fått" +
                                            " en dødelig utgang. Harry Potter er nemlig en trollmann!" +
                                            " Dette er den første boken om Harry Potter."));
        }


      

    }
}
