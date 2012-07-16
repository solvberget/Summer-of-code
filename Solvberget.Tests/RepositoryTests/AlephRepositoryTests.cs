using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    public class AlephRepositoryTests
    {

        private AlephRepository _repository;
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");

        [SetUp]
        public void InitRepository()
        {

            _repository = new AlephRepository(_imageCache);
        }

        [Test]
        public void TestFind()
        {
            const string searchString = "naiv super";
            var documents = _repository.Search(searchString);

            var books = documents.Where(x => x.GetType().Name.Equals("Book"));

            Assert.AreEqual(7, books.Count());
            Assert.AreEqual(0, documents.Count(x => x.GetType().Name.Equals("Document")));
        }

        [Test]
        public void TestGetBook()
        {
            const string documentNumberForBook = "000596743"; //Naiv. Super
            var book = (Book)_repository.GetDocument(documentNumberForBook, false);
            Assert.AreEqual("Book", book.GetType().Name);
            Assert.AreEqual("Naiv. Super", book.Title);
            Assert.AreEqual("Loe, Erlend", book.Author.Name);
            Assert.AreEqual("978-82-02-33225-9", book.Isbn);
            Assert.AreEqual("205 s.", book.NumberOfPages);
            Assert.AreEqual("LOE", book.LocationCode);
            Assert.AreEqual(2010, book.PublishedYear);
            Assert.AreEqual("[Oslo]", book.PlacePublished);
        }

        [Test]
        public void TestGetFilm()
        {
            const string documentNumberForFilm = "000604643"; //The Matrix
            var film = (Film)_repository.GetDocument(documentNumberForFilm, false);
            Assert.AreEqual("Film", film.GetType().Name);
            Assert.AreEqual("The matrix revolutions", film.Title);
            Assert.AreEqual("2003", film.ProductionYear);
            Assert.AreEqual("7321900680592", film.Ean);
            Assert.AreEqual("Fiksjon", film.IsFiction);
            Assert.AreEqual("Flerspråklig", film.Language);
            Assert.AreEqual(2, film.Languages.Count());
            Assert.AreEqual("Engelsk", film.Languages.ElementAt(0));
            Assert.AreEqual("Tysk", film.Languages.ElementAt(1));
        }

        [Test]
        public void TestGetAudioBook()
        {
            const string documentNumberForAudioBook = "000599186"; //Harry Potter og dødtalismanene
            var audioBook = (AudioBook)_repository.GetDocument(documentNumberForAudioBook, false);
            Assert.AreEqual("AudioBook", audioBook.GetType().Name);
            Assert.AreEqual("978-82-02-29195-2", audioBook.Isbn);
            Assert.AreEqual("Rowling, J.K.", audioBook.Author.Name);
            Assert.AreEqual("Fiksjon", audioBook.IsFiction);
            Assert.AreEqual("Harry Potter", audioBook.SeriesTitle);
        }

        [Test]
        public void TestGetNonExistingDoc()
        {
            const string documentNumberForBook = "abcdefg"; //Burde ikke funke
            var doc = _repository.GetDocument(documentNumberForBook, false);
            Assert.IsNull(doc);
        }

        [Test]
        public void TestGetDocumentsLightCountAndContent()
        {
            IEnumerable<string> books = new string[] { "000588841", "000588844", "000588843", "000598029", "000567325" };
            var result = _repository.GetDocumentsLight(books.Take(1));
            var firstResult = (Film)result.ElementAt(0);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Ringenes herre : Atter en konge", firstResult.Title);
            Assert.AreEqual(2010, firstResult.PublishedYear);
            Assert.AreEqual("The Lord of the rings", firstResult.OriginalTitle);

        }

        [Test]
        public void TestGetDocumentsLightCount()
        {
            IEnumerable<string> books = new string[] { "000588841", "000588844", "000588843", "000598029", "000567325" };
            var result = _repository.GetDocumentsLight(books);
            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public void TestGetDocumentsLightInvalidIds()
        {
            IEnumerable<string> invalidIds = new string[] { "abs2ls", "000123lkjsdf" };
            var result = _repository.GetDocumentsLight(invalidIds);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void TestGetDocumentsLightMixOfValidAndInvalidIds()
        {
            IEnumerable<string> mixedIds = new string[] { "000588841", "abs2ls", "000588844", "000588843", "000123lkjsdf", "000598029", "000567325" };
            var result = _repository.GetDocumentsLight(mixedIds);
            Assert.AreEqual(5, result.Count);
        }

    }
}
