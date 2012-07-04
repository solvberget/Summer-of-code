using System;
using NUnit.Framework;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    class ImageRepositoryTest
    {

        private ImageRepository _imageRepository;

        [SetUp]
        public void InitRepository()
        {
            _imageRepository = new ImageRepository();
        }

        [Test]
        public void TestGetDocImage()
        {

            Assert.IsNullOrEmpty(_imageRepository.GetDocumentImage(""));
            Assert.IsNullOrEmpty(_imageRepository.GetDocumentImage("asdsadasd"));


            // 000605680 - Harry Potter and the philosophers stone (book)
            const string harryPotterBook = "000610109";
            var hpBook = _imageRepository.GetDocumentImage(harryPotterBook);
            Assert.IsNullOrEmpty(hpBook);

            
            // 000605680 - Harry Potter og dødstalismanene
            const string harryPotterMovie = "000605680";
            var hpMovieImage = _imageRepository.GetDocumentImage(harryPotterMovie);
            Assert.IsNotNullOrEmpty(hpMovieImage);
            Assert.True(hpMovieImage.Contains("http"));
            Assert.True(hpMovieImage.Equals("http://ia.media-imdb.com/images/M/MV5BMTQ2OTE1Mjk0N15BMl5BanBnXkFtZTcwODE3MDAwNA@@._V1_SX640.jpg"));

            // 000579526 - Istid 3
            const string theDawnOfTheIceAgeMovie = "000579526";
            var iceAgeMovie = _imageRepository.GetDocumentImage(theDawnOfTheIceAgeMovie);
            Assert.IsNotNullOrEmpty(iceAgeMovie);
            Assert.True(iceAgeMovie.Contains("http"));
            Assert.True(iceAgeMovie.Equals("http://ia.media-imdb.com/images/M/MV5BMjEyNzI1ODA0MF5BMl5BanBnXkFtZTYwODIxODY3._V1_SX640.jpg"));


            // 000588841 - Atter en konge
            const string atterEnKongeMovie = "000588841";
            var rhMovie = _imageRepository.GetDocumentImage(atterEnKongeMovie);
            Assert.IsNotNullOrEmpty(rhMovie);
            Assert.True(rhMovie.Contains("http"));
            Assert.True(rhMovie.Equals("http://ia.media-imdb.com/images/M/MV5BMjE4MjA1NTAyMV5BMl5BanBnXkFtZTcwNzM1NDQyMQ@@._V1_SX640.jpg"));

            // 000418201 - Mongoland
            const string mongolandMovie = "000418201";
            var mongoMovie = _imageRepository.GetDocumentImage(mongolandMovie);
            Assert.IsNotNullOrEmpty(mongoMovie);
            Assert.True(rhMovie.Contains("http"));
            Assert.True(mongoMovie.Equals("http://ia.media-imdb.com/images/M/MV5BNDk4NTc2NjcwNV5BMl5BanBnXkFtZTcwNjU1MDY5MQ@@._V1_SX640.jpg"));

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
