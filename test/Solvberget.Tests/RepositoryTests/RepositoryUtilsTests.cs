using System;
using System.IO;

using Solvberget.Domain.Utils;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    class RepositoryUtilsTests
    {
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");

        public RepositoryUtilsTests()
        {
            
        }

        [Fact]
        public void TestGetJsonFromStreamWithParam()
        {
            var imdbObjectAsJson = RepositoryUtils.GetJsonFromStreamWithParam("http://www.imdbapi.com/?t=", "Harry Potter");
            if( !string.IsNullOrEmpty(imdbObjectAsJson) )
            {
                Assert.True(imdbObjectAsJson.Contains("Harry Potter"));
            }
        }

        [Fact]
        public void TestDownloadImageFromUrl()
        {
            // Delete cache folder if it exists
            if (Directory.Exists(_imageCache))
                Directory.Delete(_imageCache, true);

            ImageCacheUtils.DownloadImageFromUrl("http://ia.media-imdb.com/images/M/MV5BNzU3NDg4NTAyNV5BMl5BanBnXkFtZTcwOTg2ODg1Mg@@._V1_SX640.jpg","MV5BNzU3NDg4NTAyNV5BMl5BanBnXkFtZTcwOTg2ODg1Mg@@._V1_SX640.jpg", _imageCache );


            // Check if cache folder exists
            Assert.True(Directory.Exists(_imageCache));

            // Check if file exists
            Assert.True(File.Exists(_imageCache + "MV5BNzU3NDg4NTAyNV5BMl5BanBnXkFtZTcwOTg2ODg1Mg@@._V1_SX640.jpg"));
        }
    }
}
