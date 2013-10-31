using System.Collections.Generic;

using FakeItEasy;
using Nancy;
using Nancy.Testing;

using Should;
using Solvberget.Core.DTOs;
using Solvberget.Domain;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Documents;
using Solvberget.Domain.Documents.Images;
using Solvberget.Domain.Documents.Ratings;
using Solvberget.Domain.Documents.Reviews;
using Solvberget.Nancy.Modules;

using Xunit;

namespace Solvberget.Nancy.Tests
{
    public class DocumentModuleTests
    {
        private readonly IRepository _documentRepository;

        private readonly IImageRepository _imageRepository;

        private readonly IRatingRepository _ratingRepository;

        private readonly IReviewRepository _reviewRepository;

        private readonly Browser _browser;

        public DocumentModuleTests()
        {
            _documentRepository = A.Fake<IRepository>();
            _imageRepository = A.Fake<IImageRepository>();
            _ratingRepository = A.Fake<IRatingRepository>();
            _reviewRepository = A.Fake<IReviewRepository>();

            _browser = new Browser(with =>
            {
                with.Module<DocumentModule>();
                with.Dependency(_documentRepository);
                with.Dependency(_imageRepository);
                with.Dependency(_ratingRepository);
                with.Dependency(_reviewRepository);
            });
        }

        [Fact(Skip="Search is not yet implemented in refactored API")]
        public void SearchShouldQueryDocumentRepository()
        {
            // Given
            A.CallTo(() => _documentRepository.Search("Harry Potter")).Returns(new List<Document>
            {
                new Book
                {
                    Author = new Person { Name = "J. K. Rowling" },
                    Title = "Harry Potter and the Chamber of Secrets"
                },
                new Book
                {
                    Author = new Person { Name = "J. K. Rowling" },
                    Title = "Harry Potter and the Philosopher's Stone"
                }
            });

            // When
            var response = _browser.Get("/documents/search", with =>
            {
                with.Query("q", "Harry Potter");
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<List<Document>>().Count.ShouldEqual(2);
        }

        [Fact]
        public void GetWithIdShouldFetchDocumentFromRepository()
        {
            // Given
            A.CallTo(() => _documentRepository.GetDocument("1234", false)).Returns(new Cd { Title = "Black Album" });

            // When
            var response = _browser.Get("/documents/1234", with =>
            {
                with.Query("light", "true");
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<DocumentDto>().Title.ShouldEqual("Black Album");
        }

        [Fact]
        public void GetReviewShouldFetchReviewFromRepository()
        {
            // Given
            A.CallTo(() => _reviewRepository.GetDocumentReview("1234")).Returns("Awesome!");

            // When
            var response = _browser.Get("/documents/1234/review", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();       
            });

            // Then
            response.Body.DeserializeJson<DocumentReviewDto>().Review.ShouldEqual("Awesome!");
        }

        [Fact]
        public void GetRatingShouldFetchRatingFromRepository()
        {
            var rating = new DocumentRating(){Score = 10};
            // Given
            A.CallTo(() => _ratingRepository.GetDocumentRating("1234")).Returns(rating);

            // When
            var response = _browser.Get("/documents/1234/rating", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<DocumentRating>().Score.ShouldEqual(rating.Score);
        }

        [Fact]
        public void GetThumbnailShouldRedirectToThumbnail()
        {
            // Given
            A.CallTo(() => _imageRepository.GetDocumentImage("1234")).Returns("image.png");

            // When
            var response = _browser.Get("/documents/1234/thumbnail", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.StatusCode.ShouldEqual(HttpStatusCode.SeeOther);
            response.Headers.Keys.ShouldContain("Location");
            response.Headers["Location"].ShouldEqual("image.png");
        }
    }
}