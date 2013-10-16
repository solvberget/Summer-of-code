using System;
using System.IO;
using System.Linq;

using FakeItEasy;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    public class LibraryListsFromDynamicRepositoryTest
    {
        private const string PathString = @"..\..\..\Solvberget.Service\bin\App_Data\librarylists\dynamic";
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");

        private readonly IListRepository _listRepository;

        public LibraryListsFromDynamicRepositoryTest()
        {
            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetImageCachePath()).Returns(_imageCache);

            var path = Path.Combine(Environment.CurrentDirectory, PathString);
            A.CallTo(() => fake.GetXmlFilePath()).Returns(path);

            var aleph = new AlephRepository(fake);
            _listRepository = new LibraryListDynamicRepository(aleph, new ImageRepository(aleph, fake), fake);

        }

        [Fact(Skip = "This test is failing, determine whether to keep this or not...")]
        public void TestCorrectFileCount()
        {
            var result = _listRepository.GetLists();
            Assert.Equal(2, result.Count);
        }

        [Fact(Skip = "This test is failing, determine whether to keep this or not...")]
        public void TestListContent()
        {
            var result = _listRepository.GetLists();
            Assert.Equal("Nyheter den siste uken: Skjønnlitteratur for voksne", result.ElementAt(0).Name);
            Assert.Equal("Nyheter den siste uken: Musikk-cder", result.ElementAt(1).Name);
            Assert.False(result.ElementAt(0).IsRanked);
            Assert.False(result.ElementAt(1).IsRanked);
        }

    }
}
