using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    internal class LibraryListsFromXmlRepositoryTest
    {
        private const string PathString = @"..\..\..\Solvberget.Service\bin\App_Data\librarylists\static";
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");

        private LibraryListXmlRepository _listRepository;

        [TestFixtureSetUp]
        public void Init()
        {
            var path = Path.Combine(Environment.CurrentDirectory, PathString);
            var aleph = new AlephRepository(_imageCache);
            _listRepository = new LibraryListXmlRepository(aleph, new ImageRepository(aleph, _imageCache), path);
        }

        [Test]
        public void TestCorrectFileCount()
        {
            var result = _listRepository.GetLists();
            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public void TestListNameAndDocumentNumbers()
        {
            var list = _listRepository.GetLists().First();
            Assert.AreEqual("Musikk+Filmbiblioteket anbefaler: Film", list.Name);
            Assert.AreEqual(1, list.Priority);
            Assert.AreEqual(5, list.DocumentNumbers.Count);
            Assert.AreEqual("000609967", list.DocumentNumbers.ElementAt(0));
            Assert.AreEqual("000600109", list.DocumentNumbers.ElementAt(1));
            Assert.AreEqual("000611985", list.DocumentNumbers.ElementAt(2));
            Assert.AreEqual("000539501", list.DocumentNumbers.ElementAt(3));
            Assert.AreEqual("000609384", list.DocumentNumbers.ElementAt(4));
        }

        [Test]
        public void TestGetListsWithLimit()
        {
            var lists1 = _listRepository.GetLists(2);
            Assert.AreEqual(2, lists1.Count);
            Assert.AreEqual("Musikk+Filmbiblioteket anbefaler: Film", lists1.ElementAt(0).Name);
            Assert.AreEqual(1, lists1.ElementAt(0).Priority);
            Assert.AreEqual("Ferske lesetips fra Sølvbergets ansatte - Skjønnlitteratur", lists1.ElementAt(1).Name);
            Assert.AreEqual(2, lists1.ElementAt(1).Priority);

            //Case where limit is higher than file conunt in folder
            var lists2 = _listRepository.GetLists(6);
            Assert.AreEqual(5, lists2.Count);
        }

        [Test]
        public void TestListContent()
        {
            var list = _listRepository.GetLists().First();
            var document = list.Documents.ElementAt(0);
            Assert.AreEqual(5, list.Documents.Count());
            Assert.AreEqual("Musikk+Filmbiblioteket anbefaler: Film", list.Name);
            Assert.AreEqual("Apejentene", document.Title);
            Assert.AreEqual(2012, document.PublishedYear);
            Assert.AreEqual("Film", document.DocType);
        }

        [Test]
        public void TestIfListRanked()
        {
            var result = _listRepository.GetLists();
            Assert.IsFalse(result.ElementAt(0).IsRanked);
            Assert.IsTrue(result.ElementAt(3).IsRanked);
        }

    }
}
