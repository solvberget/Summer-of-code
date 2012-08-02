using System;
using System.IO;
using NUnit.Framework;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    class RatingRepositoryTest
    {

        private RatingRepository _ratingRepository;
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");


      [SetUp]
        public void InitRepository()
        {

            _ratingRepository = new RatingRepository(new AlephRepository(_imageCache));

        }

        [Test]
        public void TestGetDocumentRating()
        {
            
            // 000605680 - Harry Potter og dødstalismanene del 2, size: default (150)
            const string harryPotterMovie = "000605680";
            var hpMovieRating = _ratingRepository.GetDocumentRating(harryPotterMovie);
            Assert.IsNotNullOrEmpty(hpMovieRating);
         //   Assert.True(hpMovieRating.Equals("8.1"));


        }

    }
}
