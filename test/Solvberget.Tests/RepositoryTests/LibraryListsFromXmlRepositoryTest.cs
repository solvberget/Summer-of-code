using System;
using System.IO;
using System.Linq;

using FakeItEasy;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    internal class LibraryListsFromXmlRepositoryTest
    {
        private const string PathString = @"..\..\..\Solvberget.Service\bin\App_Data\librarylists\static";
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");

        private readonly IListRepositoryStatic _listRepository;

        public LibraryListsFromXmlRepositoryTest()
        {
            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetImageCachePath()).Returns(_imageCache);

            var path = Path.Combine(Environment.CurrentDirectory, PathString);
            A.CallTo(() => fake.GetXmlListPath()).Returns(path);

            var aleph = new AlephRepository(fake);
            _listRepository = new LibraryListXmlRepository(aleph, new ImageRepository(aleph, fake), fake);

        }

        [Fact]
        public void TestCorrectFileCount()
        {
            var result = _listRepository.GetLists();
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void TestListNameAndDocumentNumbers()
        {
            var list = _listRepository.GetLists().First();
            Assert.Equal("Musikk+Filmbiblioteket anbefaler: Film", list.Name);
            Assert.Equal(1, list.Priority);
            Assert.Equal(5, list.DocumentNumbers.Count);
            Assert.Equal("000609967", list.DocumentNumbers.ElementAt(0).Key);
            Assert.Equal("000600109", list.DocumentNumbers.ElementAt(1).Key);
            Assert.Equal("000611985", list.DocumentNumbers.ElementAt(2).Key);
            Assert.Equal("000539501", list.DocumentNumbers.ElementAt(3).Key);
            Assert.Equal("000609384", list.DocumentNumbers.ElementAt(4).Key);
            Assert.False(list.DocumentNumbers.ElementAt(0).Value);
            Assert.False(list.DocumentNumbers.ElementAt(1).Value);
            Assert.False(list.DocumentNumbers.ElementAt(2).Value);
            Assert.False(list.DocumentNumbers.ElementAt(3).Value);
            Assert.False(list.DocumentNumbers.ElementAt(4).Value);
        }

        [Fact]
        public void TestGetListsWithLimit()
        {
            var lists1 = _listRepository.GetLists(2);
            Assert.Equal(2, lists1.Count);
            Assert.Equal("Musikk+Filmbiblioteket anbefaler: Film", lists1.ElementAt(0).Name);
            Assert.Equal(1, lists1.ElementAt(0).Priority);
            Assert.Equal("Ferske lesetips fra Sølvbergets ansatte - Skjønnlitteratur", lists1.ElementAt(1).Name);
            Assert.Equal(2, lists1.ElementAt(1).Priority);

            //Case where limit is higher than file conunt in folder
            var lists2 = _listRepository.GetLists(6);
            Assert.Equal(5, lists2.Count);
        }

        [Fact]
        public void TestIfListRanked()
        {
            var result = _listRepository.GetLists();
            Assert.False(result.ElementAt(0).IsRanked);
            Assert.True(result.ElementAt(3).IsRanked);
        }
    }
}
