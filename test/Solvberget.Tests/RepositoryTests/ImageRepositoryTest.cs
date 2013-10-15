using System;
using System.IO;

using FakeItEasy;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    
    class ImageRepositoryTest
    {

        private ImageRepository _imageRepository;
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");
        private const string ServerCacheImagesPath = "http://localhost:7089/Content/cacheImages/";

        public ImageRepositoryTest()
        {
            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetImageCachePath()).Returns(_imageCache);

            _imageRepository = new ImageRepository(new AlephRepository(fake), fake);  
        }

        [Fact]
        public void TestGetDocumentThumbnailImage()
        {
            // 000605680 - Harry Potter og dødstalismanene del 2, size: default (150)
            const string harryPotterMovie = "000605680";
            var hpMovieImage = _imageRepository.GetDocumentThumbnailImage(harryPotterMovie, null);
            Assert.NotNull(hpMovieImage);
            Assert.True(hpMovieImage.Equals(ServerCacheImagesPath + "thumb000605680.jpg"));

            // 000605680 - Harry Potter og dødstalismanene del 2, size: 100
            hpMovieImage = _imageRepository.GetDocumentThumbnailImage(harryPotterMovie, "100");
            Assert.NotNull(hpMovieImage);
            Assert.Equal(ServerCacheImagesPath + "thumb000605680-100.jpg", hpMovieImage);

        }


        [Fact]
        public void TestLocalThumbnailImageCache()
        {

            // Delete cache folder if it exists
            if ( Directory.Exists(_imageCache))
                Directory.Delete(_imageCache, true);

            // 000605680 - Harry Potter og dødstalismanene del 2, size: default thumb (150)
            const string harryPotterMovie = "000605680";
            var hpMovieImage = _imageRepository.GetDocumentThumbnailImage(harryPotterMovie, null);
            Assert.NotNull(hpMovieImage);

            // Check if cache folder exists
            Assert.True(Directory.Exists(_imageCache));

            // Check if file exists
            Assert.True(File.Exists(_imageCache + "thumb000605680.jpg"));

            hpMovieImage = _imageRepository.GetDocumentThumbnailImage(harryPotterMovie, "120");
            Assert.NotNull(hpMovieImage);

            // Check if file exists
            Assert.True(File.Exists(_imageCache + "thumb000605680-120.jpg"));



        }

        [Fact]
        public void TestLocalImageCache()
        {

            // Delete cache folder if it exists
            if (Directory.Exists(_imageCache))
                Directory.Delete(_imageCache, true);

            // 000605680 - Harry Potter og dødstalismanene del 2
            const string harryPotterMovie = "000605680";
            var hpMovieImage = _imageRepository.GetDocumentImage(harryPotterMovie);
            Assert.NotNull(hpMovieImage);

            // Check if cache folder exists
            Assert.True(Directory.Exists(_imageCache));

            // Check if file exists
            Assert.True(File.Exists(_imageCache + "000605680.jpg"));

        }

        [Fact]
        public void TestGetDocImage()
        {

            // Delete cache folder if it exists
            if (Directory.Exists(_imageCache))
                Directory.Delete(_imageCache, true);

            Assert.Null(_imageRepository.GetDocumentImage(""));
            Assert.Null(_imageRepository.GetDocumentImage("asdsadasd"));

            // 000605680 - Harry Potter and the philosophers stone (book)
            const string harryPotterBook = "000610109";
            var hpBook = _imageRepository.GetDocumentImage(harryPotterBook);
            Assert.NotNull(hpBook);
            Assert.Equal(hpBook, ServerCacheImagesPath + "000610109.jpg");
           
            // 000605680 - Harry Potter og dødstalismanene del 2
            const string harryPotterMovie = "000605680";
            var hpMovieImage = _imageRepository.GetDocumentImage(harryPotterMovie);
            Assert.NotNull(hpMovieImage);
            Assert.True(hpMovieImage.Equals(ServerCacheImagesPath + "000605680.jpg"));

  

            // 000588841 - Atter en konge
            const string atterEnKongeMovie = "000588841";
            var rhMovie = _imageRepository.GetDocumentImage(atterEnKongeMovie);
            Assert.NotNull(rhMovie);
            Assert.True(rhMovie.Equals(ServerCacheImagesPath + "000588841.jpg"));

            // 000418201 - Mongoland
            const string mongolandMovie = "000418201";
            var mongoMovie = _imageRepository.GetDocumentImage(mongolandMovie);
            Assert.NotNull(mongoMovie);
            Assert.True(mongoMovie.Equals(ServerCacheImagesPath + "000418201.jpg"));

            // 000605883 - Den hemmelighetsfulle leiligheten
            const string randomIbsenMovie = "000605883";
            var ibsenMovie = _imageRepository.GetDocumentImage(randomIbsenMovie);
            Assert.Null(ibsenMovie);

            // 000600766 - Olsenbanden jr. på cirkus ; Olsenbanden jr. på rockern ; Olsenbanden jr. går under vann
            // Serietittel: 3 filmer i 1
            // Original title: null
            const string olsenbandenJr3In1 = "000600766";
            var olsenbandenjrMovie = _imageRepository.GetDocumentImage(olsenbandenJr3In1);
            Assert.Null(olsenbandenjrMovie);
        }

    }
}
