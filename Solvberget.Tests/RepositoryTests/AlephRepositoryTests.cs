using System.Linq;
using NUnit.Framework;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    public class AlephRepositoryTests
    {



        [Test]
        public void TestFind()
        {
            const string searchString = "naiv super";
            var repository = new AlephRepository();
            var documents = repository.Search(searchString);

            var books = documents.Where(x => x.GetType().Name.Equals("Book"));

            Assert.AreEqual(7, books.Count());
            Assert.AreEqual(3, documents.Count(x => x.GetType().Name.Equals("Document")));
        }

        [Test]
        public void TestGetDocument()
        {
            const string documentNumberForBook = "000596743"; //Naiv. Super
            const string documentNumberForFilm = "000604643"; //The Matrix
            const string documentNumberForAudioBook = "000599186"; //Harry Potter og dødtalismanene
            var repository = new AlephRepository();
            
            var book = (Book)repository.GetDocument(documentNumberForBook);
            Assert.AreEqual("Book", book.GetType().Name);
            Assert.AreEqual("Naiv. Super", book.Title);
            Assert.AreEqual("Loe, Erlend", book.Author.Name);
            Assert.AreEqual("978-82-02-33225-9", book.Isbn);
            Assert.AreEqual("205 s.", book.NumberOfPages);
            Assert.AreEqual("LOE", book.LocationCode);
            Assert.AreEqual(2010, book.PublishedYear);
            Assert.AreEqual("[Oslo]", book.PlacePublished);
            
            var film = (Film)repository.GetDocument(documentNumberForFilm);
            Assert.AreEqual("Film", film.GetType().Name);
            Assert.AreEqual("The matrix revolutions", film.Title);
            Assert.AreEqual("2003", film.ProductionYear);
            Assert.AreEqual("7321900680592", film.Ean);
            Assert.IsTrue(film.IsFiction);
            Assert.AreEqual("mul", film.Language);
            Assert.AreEqual(2, film.Languages.Count());
            Assert.AreEqual("eng", film.Languages.ElementAt(0));
            Assert.AreEqual("ger", film.Languages.ElementAt(1));

            var audioBook = (AudioBook) repository.GetDocument(documentNumberForAudioBook);
            Assert.AreEqual("AudioBook", audioBook.GetType().Name);

        }



    }
}
