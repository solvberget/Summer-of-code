using System;
using System.IO;

using FakeItEasy;

using NUnit.Framework;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    class ImageRepositoryTest
    {

        private ImageRepository _imageRepository;
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");
        private const string ServerCacheImagesPath = "http://localhost:7089/Content/cacheImages/";
        
        [SetUp]
        public void InitRepository()
        {
            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetImageCachePath()).Returns(_imageCache);

            _imageRepository = new ImageRepository(new AlephRepository(fake), fake);
        }

        [Test]
        public void TestGetDocumentThumbnailImage()
        {
            // 000605680 - Harry Potter og dødstalismanene del 2, size: default (150)
            const string harryPotterMovie = "000605680";
            var hpMovieImage = _imageRepository.GetDocumentThumbnailImage(harryPotterMovie, null);
            Assert.IsNotNullOrEmpty(hpMovieImage);
            Assert.True(hpMovieImage.Equals(ServerCacheImagesPath + "thumb000605680.jpg"));

            // 000605680 - Harry Potter og dødstalismanene del 2, size: 100
            hpMovieImage = _imageRepository.GetDocumentThumbnailImage(harryPotterMovie, "100");
            Assert.IsNotNullOrEmpty(hpMovieImage);
            Assert.AreEqual(ServerCacheImagesPath + "thumb000605680-100.jpg", hpMovieImage);

        }


        [Test]
        public void TestLocalThumbnailImageCache()
        {

            // Delete cache folder if it exists
            if ( Directory.Exists(_imageCache))
                Directory.Delete(_imageCache, true);

            // 000605680 - Harry Potter og dødstalismanene del 2, size: default thumb (150)
            const string harryPotterMovie = "000605680";
            var hpMovieImage = _imageRepository.GetDocumentThumbnailImage(harryPotterMovie, null);
            Assert.IsNotNullOrEmpty(hpMovieImage);

            // Check if cache folder exists
            Assert.True(Directory.Exists(_imageCache));

            // Check if file exists
            Assert.True(File.Exists(_imageCache + "thumb000605680.jpg"));

            hpMovieImage = _imageRepository.GetDocumentThumbnailImage(harryPotterMovie, "120");
            Assert.IsNotNullOrEmpty(hpMovieImage);

            // Check if file exists
            Assert.True(File.Exists(_imageCache + "thumb000605680-120.jpg"));



        }

        [Test]
        public void TestLocalImageCache()
        {

            // Delete cache folder if it exists
            if (Directory.Exists(_imageCache))
                Directory.Delete(_imageCache, true);

            // 000605680 - Harry Potter og dødstalismanene del 2
            const string harryPotterMovie = "000605680";
            var hpMovieImage = _imageRepository.GetDocumentImage(harryPotterMovie);
            Assert.IsNotNullOrEmpty(hpMovieImage);

            // Check if cache folder exists
            Assert.True(Directory.Exists(_imageCache));

            // Check if file exists
            Assert.True(File.Exists(_imageCache + "000605680.jpg"));

        }

        [Test]
        public void TestGetDocImage()
        {

            // Delete cache folder if it exists
            if (Directory.Exists(_imageCache))
                Directory.Delete(_imageCache, true);

            Assert.IsNullOrEmpty(_imageRepository.GetDocumentImage(""));
            Assert.IsNullOrEmpty(_imageRepository.GetDocumentImage("asdsadasd"));


            // 000605680 - Harry Potter and the philosophers stone (book)
            const string harryPotterBook = "000610109";
            var hpBook = _imageRepository.GetDocumentImage(harryPotterBook);
            Assert.IsNotNullOrEmpty(hpBook);
            Assert.AreEqual(hpBook, ServerCacheImagesPath + "000610109.jpg");
           
            // 000605680 - Harry Potter og dødstalismanene del 2
            const string harryPotterMovie = "000605680";
            var hpMovieImage = _imageRepository.GetDocumentImage(harryPotterMovie);
            Assert.IsNotNullOrEmpty(hpMovieImage);
            Assert.True(hpMovieImage.Equals(ServerCacheImagesPath + "000605680.jpg"));

  

            // 000588841 - Atter en konge
            const string atterEnKongeMovie = "000588841";
            var rhMovie = _imageRepository.GetDocumentImage(atterEnKongeMovie);
            Assert.IsNotNullOrEmpty(rhMovie);
            Assert.True(rhMovie.Equals(ServerCacheImagesPath + "000588841.jpg"));

            // 000418201 - Mongoland
            const string mongolandMovie = "000418201";
            var mongoMovie = _imageRepository.GetDocumentImage(mongolandMovie);
            Assert.IsNotNullOrEmpty(mongoMovie);
            Assert.True(mongoMovie.Equals(ServerCacheImagesPath + "000418201.jpg"));

            // 000605883 - Den hemmelighetsfulle leiligheten
            const string randomIbsenMovie = "000605883";
            var ibsenMovie = _imageRepository.GetDocumentImage(randomIbsenMovie);
            Assert.IsNullOrEmpty(ibsenMovie);

            // 000600766 - Olsenbanden jr. på cirkus ; Olsenbanden jr. på rockern ; Olsenbanden jr. går under vann
            // Serietittel: 3 filmer i 1
            // Original title: null
            const string olsenbandenJr3In1 = "000600766";
            var olsenbandenjrMovie = _imageRepository.GetDocumentImage(olsenbandenJr3In1);
            Assert.IsNullOrEmpty(olsenbandenjrMovie);

        }

    }
}
