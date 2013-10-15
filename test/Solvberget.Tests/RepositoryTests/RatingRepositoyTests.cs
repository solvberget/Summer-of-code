using System;
using System.IO;

using FakeItEasy;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    
    internal class RatingRepositoryTest
    {
        private readonly RatingRepository _ratingRepository;

        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory,
            @"..\..\..\Solvberget.Service\Content\cacheImages\");

        public RatingRepositoryTest()
        {
            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetImageCachePath()).Returns(_imageCache);

            _ratingRepository = new RatingRepository(new AlephRepository(fake));
        }

        [Fact]
        public void TestGetDocumentRating()
        {
            // 000605680 - Harry Potter og dødstalismanene del 2, size: default (150)
            const string harryPotterMovie = "000605680";
            var hpMovieRating = _ratingRepository.GetDocumentRating(harryPotterMovie);
            Assert.NotNull(hpMovieRating);
            //   Assert.True(hpMovieRating.Equals("8.1"));
        }
    }
}