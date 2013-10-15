using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using FakeItEasy;

using NUnit.Framework;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    public class AlephRepositoryTests
    {

        private AlephRepository _repository;
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");
        private readonly string _pathToRulesFolder = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\bin\App_Data\rules\");

        [SetUp]
        public void InitRepository()
        {
            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetImageCachePath()).Returns(_imageCache);
            A.CallTo(() => fake.GetRulesPath()).Returns(_pathToRulesFolder);

            _repository = new AlephRepository(fake);
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
            Assert.AreEqual("Erlend Loe", book.Author.Name);
            Assert.AreEqual("978-82-02-33225-9", book.Isbn);
            Assert.AreEqual("205 s.", book.NumberOfPages);
            Assert.AreEqual("LOE", book.LocationCode);
            Assert.AreEqual(2010, book.PublishedYear);
            Assert.AreEqual("[Oslo]", book.PlacePublished);
            Assert.AreEqual("Bok (2010)", book.CompressedSubTitle);

        }

        [Test]
        [Ignore("Unnecessary integration test")]
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
            Assert.AreEqual("Film (2004)", film.CompressedSubTitle);
        }

        [Test]
        [Ignore("Unnecessary integration test")]
        public void TestGetAudioBook()
        {
            const string documentNumberForAudioBook = "000599186"; //Harry Potter og dødtalismanene
            var audioBook = (AudioBook)_repository.GetDocument(documentNumberForAudioBook, false);
            Assert.AreEqual("AudioBook", audioBook.GetType().Name);
            Assert.AreEqual("978-82-02-29195-2", audioBook.Isbn);
            Assert.AreEqual("J.K. Rowling", audioBook.Author.Name);
            Assert.AreEqual("Fiksjon", audioBook.IsFiction);
            Assert.AreEqual("Harry Potter", audioBook.SeriesTitle);
            Assert.AreEqual("Lydbok (2008)", audioBook.CompressedSubTitle);

        }

        [Test]
        [Ignore("Unnecessary integration test")]
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
            Assert.AreEqual("Tidsskrift (19)", journal.CompressedSubTitle);

        }

        [Test]
        [Ignore("Unnecessary integration test")]
        public void TestGetCdPopular()
        {
            const string documentNumberForCdPopular = "000566205"; //Mods - Gje meg litt merr
            var cd = (Cd)_repository.GetDocument(documentNumberForCdPopular, false);
            Assert.AreEqual("Gje meg litt merr", cd.Title);
            Assert.AreEqual("Mods", cd.MusicGroup);
            Assert.IsNull(cd.ExplanatoryAddition);
            Assert.AreEqual("1 kompaktplate", cd.TypeAndNumberOfDiscs);
            Assert.AreEqual("Innhold: Gje meg litt merr ; Belinda ; Revansj ; Me to går alltid aleina ; Amerika ; Bare i nått ; Eg e så forelska ; Ett år e gått ; Tore Tang ; Fint at du vil ; Eg vil hjem ; Hjelp meg ; Militæret ; Alexander ; Eg kom ikkje inn ; Regn ; Meg må du hilsa på ; Ikkje plag meg ; Bli med oss ; Livets roulette ; Another day ; Bahama Mama", cd.DiscContent);
            Assert.AreEqual("Utøvere: Kurt Ø. Olsen, Helge Hummervoll, Leif Nilsen, Morten A. Knutsen, Torkild Viig, Runar Bjaalid, Tor Øyvind Syvertsen", cd.Performers);
            Assert.AreEqual(2, cd.CompositionTypeOrGenre.Count());
            Assert.AreEqual("Popmusikk", cd.CompositionTypeOrGenre.ElementAt(0));
            Assert.AreEqual("Rock", cd.CompositionTypeOrGenre.ElementAt(1));
            Assert.AreEqual(7, cd.InvolvedPersons.Count());
            Assert.IsEmpty(cd.InvolvedMusicGroups);
            Assert.AreEqual("Cd (2006)", cd.CompressedSubTitle);

        }

        [Test]
        [Ignore("Unnecessary integration test")]
        public void TestGetLanguageCourse()
        {
            const string documentNumberForLanguageCourse = "000391825"; //Jeg snakker norsk språkkurs
            var languageCourse = (LanguageCourse)_repository.GetDocument(documentNumberForLanguageCourse, false);
            Assert.AreEqual("Nils Ingnes", languageCourse.Author.Name);
            Assert.AreEqual("1941-", languageCourse.Author.LivingYears);
            Assert.AreEqual("Norsk", languageCourse.Author.Nationality);
            Assert.AreEqual("439.683", languageCourse.ClassificationNr);
            Assert.AreEqual(0, languageCourse.InvolvedOrganizations.Count());
            Assert.AreEqual(0, languageCourse.InvolvedPersons.Count());
            Assert.AreEqual("Engelsk", languageCourse.Language);
            Assert.AreEqual("Språkkurs", languageCourse.Subject.ElementAt(0));
            Assert.AreEqual("Norsk", languageCourse.Subject.ElementAt(1));
            Assert.AreEqual(2, languageCourse.Subject.Count());
            Assert.AreEqual(null, languageCourse.TitlesOtherWritingForms);
            Assert.AreEqual("4 CD plater og 1 veiledningshefte", languageCourse.TypeAndNumberOfDiscs);
            Assert.AreEqual("LanguageCourse", languageCourse.DocType);
            Assert.AreEqual("Språkkurs, Nils Ingnes (1999)", languageCourse.CompressedSubTitle);

        }

        [Test]
        [Ignore("Unnecessary integration test")]
        public void TestGetSheetMusic()
        {
            const string documentNumberForSheetMusic = "000117418"; //March (fanfare) for 3 trumpets and timpani
            var sheetMusic = (SheetMusic)_repository.GetDocument(documentNumberForSheetMusic, false);
            Assert.AreEqual("Carl Philipp Emanuel Bach", sheetMusic.Composer.Name);
            Assert.AreEqual("March (fanfare) for 3 trumpets and timpani", sheetMusic.Title);
            Assert.AreEqual("b. 4 st.", sheetMusic.NumberOfPagesAndNumberOfParts);
            Assert.AreEqual(2, sheetMusic.MusicalLineup.Count());
            Assert.AreEqual("Trompet 3", sheetMusic.MusicalLineup.ElementAt(0));
            Assert.AreEqual("Pauker", sheetMusic.MusicalLineup.ElementAt(1));
            Assert.AreEqual("Note", sheetMusic.CompressedSubTitle);

        }

        [Test]
        [Ignore("Unnecessary integration test")]
        public void TestGetNonExistingDoc()
        {
            const string documentNumberForBook = "abcdefg"; //Burde ikke funke
            var doc = _repository.GetDocument(documentNumberForBook, false);
            Assert.IsNull(doc);
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

        [Test]
        public void RequestReservationTest()
        {
            
            const string userId = "159222";
            const string docNumber = "000178569";
            const string branch = "Hovedbibl";

            var returnMessage = _repository.RequestReservation(docNumber, userId, branch);
            Assert.IsFalse(returnMessage.Success);

        }

        [Test(Description = "This is an integration test and it may fail if the document loan status changes")]
        [Ignore("Don't test dynamic data in Aleph")]
        public void TestDocumentLocation()
        {
            const string documentNumberForBook = "000596743"; //Naiv. Super
            var book = (Book)_repository.GetDocument(documentNumberForBook, false);
            Assert.AreEqual(2, book.AvailabilityInfo.Count());
            Assert.AreEqual("Hovedbibl.", book.AvailabilityInfo.ElementAt(0).Branch);
            Assert.AreEqual("Madla", book.AvailabilityInfo.ElementAt(1).Branch);
            Assert.AreEqual("Kulturbiblioteket", book.AvailabilityInfo.ElementAt(0).Department);
            Assert.AreEqual("Voksenavdelingen", book.AvailabilityInfo.ElementAt(1).Department);
            Assert.AreEqual("Skjønnlitteratur", book.AvailabilityInfo.ElementAt(0).PlacementCode);
            Assert.AreEqual("Skjønnlitteratur", book.AvailabilityInfo.ElementAt(1).PlacementCode);
        }

        [Test(Description = "This is an integration test and it may fail if the document loan status changes")]
        [Ignore("Don't test dynamic data in Aleph")]
        public void TestDocumentAvailability()
        {
            const string documentNumberForBook = "000596743"; //Naiv. Super
            var book = (Book)_repository.GetDocument(documentNumberForBook, false);
            Assert.AreEqual(1, book.AvailabilityInfo.ElementAt(0).AvailableCount);
            Assert.AreEqual(4, book.AvailabilityInfo.ElementAt(0).TotalCount);
            Assert.AreEqual(0, book.AvailabilityInfo.ElementAt(1).AvailableCount);
            Assert.AreEqual(1, book.AvailabilityInfo.ElementAt(1).TotalCount);
            Assert.NotNull(book.AvailabilityInfo.ElementAt(1).EarliestAvailableDateFormatted);
        }

        [Test(Description = "This is an integration test and it may fail if the document loan status changes")]
        [Ignore("Don't test dynamic data in Aleph")]
        public void TestNonAvailableDocument()
        {
            const string documentNumberForBook = "000611217"; //Naiv. Super
            var book = (Book)_repository.GetDocument(documentNumberForBook, false);
            Assert.AreEqual(0, book.AvailabilityInfo.ElementAt(0).AvailableCount);
            Assert.AreEqual(3, book.AvailabilityInfo.ElementAt(0).TotalCount);
            Assert.NotNull(book.AvailabilityInfo.ElementAt(0).EarliestAvailableDateFormatted);
        }

        [Test(Description = "This is an integration test and it may fail if the document loan status changes")]
        [Ignore("Don't test dynamic data in Aleph")]
        public void TestDocumentShouldOnlyIncludeOneBranch()
        {
            const string documentNumberForBook = "000530871"; //Hur kär får man bli?
            var book = (Book)_repository.GetDocument(documentNumberForBook, false);
            Assert.AreEqual(1, book.AvailabilityInfo.Count());
        }

    }

}
