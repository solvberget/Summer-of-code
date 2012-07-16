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
        public void TestGetJournal()
        {
            const string documentNumberForJournal = "000175989"; //Newsweek
            var journal = (Journal)_repository.GetDocument(documentNumberForJournal, false);
            Assert.AreEqual("Journal", journal.GetType().Name);
            Assert.AreEqual("Newsweek", journal.Title);
            Assert.AreEqual("0163-7053", journal.Issn);
            Assert.AreEqual("the international newsmagazine", journal.SubTitle);
            Assert.AreEqual("Engelsk", journal.Language);
            Assert.AreEqual("51 nummer pr. år", journal.JournalsPerYear);
        }

        [Test]
        public void TestGetCdPopular()
        {
            const string documentNumberForCdPopular = "000566205"; //Mods - Gje meg litt merr
            var cd = (CdPopular)_repository.GetDocument(documentNumberForCdPopular, false);
            Assert.AreEqual("Gje meg litt merr", cd.Title);
            Assert.AreEqual("Mods", cd.MusicGroup);
            Assert.IsNull(cd.ExplanatoryAddition);
            Assert.AreEqual("1 kompaktplate", cd.TypeAndNumberOfDiscs);
            Assert.AreEqual("Innhold: Gje meg litt merr ; Belinda ; Revansj ; Me to går alltid aleina ; Amerika ; Bare i nått ; Eg e så forelska ; Ett år e gått ; Tore Tang ; Fint at du vil ; Eg vil hjem ; Hjelp meg ; Militæret ; Alexander ; Eg kom ikkje inn ; Regn ; Meg må du hilsa på ; Ikkje plag meg ; Bli med oss ; Livets roulette ; Another day ; Bahama Mama", cd.DiscContent);
            Assert.AreEqual("Utøvere: Kurt Ø. Olsen, Helge Hummervoll, Leif Nilsen, Morten A. Knutsen, Torkild Viig, Runar Bjaalid, Tor Øyvind Syvertsen", cd.Performers);
            Assert.AreEqual(2, cd.Genre.Count());
            Assert.AreEqual("Popmusikk", cd.Genre.ElementAt(0));
            Assert.AreEqual("Rock", cd.Genre.ElementAt(1));
            Assert.AreEqual(7, cd.InvolvedPersons.Count());
            Assert.IsEmpty(cd.InvolvedMusicGroups);
        }


        [Test]
        public void TestGetLanguageCourse()
        {
            const string documentNumberForLanguageCourse = "000391825"; //Jeg snakker norsk språkkurs
            var languageCourse = (LanguageCourse)_repository.GetDocument(documentNumberForLanguageCourse, false);
            Assert.AreEqual("Ingnes, Nils", languageCourse.Author.Name);
            Assert.AreEqual("1941-", languageCourse.Author.LivingYears);
            Assert.AreEqual("Norsk", languageCourse.Author.Nationality);

            Assert.AreEqual("439.683", languageCourse.ClassificationNr);

            Assert.AreEqual(0, languageCourse.InvolvedOrganizations.Count());

            Assert.AreEqual(0, languageCourse.InvolvedPersons.Count());

            Assert.AreEqual("Engelsk", languageCourse.Language);

            Assert.AreEqual("nob", languageCourse.LearningAndTeachingLanguages);
            Assert.AreEqual(3, languageCourse.LearningAndTeachingLanguages.Count());

            Assert.AreEqual("Språkkurs", languageCourse.Subject.ElementAt(0));
            Assert.AreEqual("Norsk", languageCourse.Subject.ElementAt(1));
            Assert.AreEqual(2, languageCourse.Subject.Count());

            Assert.AreEqual(null, languageCourse.TitlesOtherWritingForms);

            Assert.AreEqual("4 CD plater og 1 veiledningshefte", languageCourse.TypeAndNumberOfDiscs);

            Assert.AreEqual("LanguageCourse", languageCourse.DocType);

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
            IEnumerable<string> books = new string[] { "000588841", "000588844",
                "000588843", "000598029", "000567325" };
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

        [Test]
        public void TestGetUserInformation()
        {

            const string userId = "159222";
            const string userVerification = "0904";

            const string wrongUserId = "sopp22";
            const string wrongUserVerification = "9999";



            var deniedUser = _repository.GetUserInformation(userId, wrongUserVerification);
            Assert.NotNull(deniedUser);
            Assert.False(deniedUser.IsAuthorized);

            deniedUser = _repository.GetUserInformation(wrongUserId, wrongUserVerification);
            Assert.NotNull(deniedUser);
            Assert.False(deniedUser.IsAuthorized);

            var authUser = _repository.GetUserInformation(userId, userVerification);
            Assert.NotNull(authUser);

            Assert.IsTrue(authUser.IsAuthorized);

        }

    }
}
