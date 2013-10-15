using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FakeItEasy;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    
    public class AlephRepositoryTests
    {
        private readonly AlephRepository _repository;
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");
        private readonly string _pathToRulesFolder = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\bin\App_Data\rules\");

        public AlephRepositoryTests()
        {
            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetImageCachePath()).Returns(_imageCache);
            A.CallTo(() => fake.GetRulesPath()).Returns(_pathToRulesFolder);

            _repository = new AlephRepository(fake);
        }

        [Fact]
        public void TestFind()
        {
            const string searchString = "naiv super";
            var documents = _repository.Search(searchString);

            var books = documents.Where(x => x.GetType().Name.Equals("Book"));

            Assert.Equal(7, books.Count());
            Assert.Equal(0, documents.Count(x => x.GetType().Name.Equals("Document")));
        }

        [Fact]
        public void TestGetBook()
        {
            const string documentNumberForBook = "000596743"; //Naiv. Super


            var book = (Book)_repository.GetDocument(documentNumberForBook, false);
            Assert.Equal("Book", book.GetType().Name);
            Assert.Equal("Naiv. Super", book.Title);
            Assert.Equal("Erlend Loe", book.Author.Name);
            Assert.Equal("978-82-02-33225-9", book.Isbn);
            Assert.Equal("205 s.", book.NumberOfPages);
            Assert.Equal("LOE", book.LocationCode);
            Assert.Equal(2010, book.PublishedYear);
            Assert.Equal("[Oslo]", book.PlacePublished);
            Assert.Equal("Bok (2010)", book.CompressedSubTitle);

        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestGetFilm()
        {
            const string documentNumberForFilm = "000604643"; //The Matrix
            var film = (Film)_repository.GetDocument(documentNumberForFilm, false);
            Assert.Equal("Film", film.GetType().Name);
            Assert.Equal("The matrix revolutions", film.Title);
            Assert.Equal("2003", film.ProductionYear);
            Assert.Equal("7321900680592", film.Ean);
            Assert.Equal("Fiksjon", film.IsFiction);
            Assert.Equal("Flerspråklig", film.Language);
            Assert.Equal(2, film.Languages.Count());
            Assert.Equal("Engelsk", film.Languages.ElementAt(0));
            Assert.Equal("Tysk", film.Languages.ElementAt(1));
            Assert.Equal("Film (2004)", film.CompressedSubTitle);
        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestGetAudioBook()
        {
            const string documentNumberForAudioBook = "000599186"; //Harry Potter og dødtalismanene
            var audioBook = (AudioBook)_repository.GetDocument(documentNumberForAudioBook, false);
            Assert.Equal("AudioBook", audioBook.GetType().Name);
            Assert.Equal("978-82-02-29195-2", audioBook.Isbn);
            Assert.Equal("J.K. Rowling", audioBook.Author.Name);
            Assert.Equal("Fiksjon", audioBook.IsFiction);
            Assert.Equal("Harry Potter", audioBook.SeriesTitle);
            Assert.Equal("Lydbok (2008)", audioBook.CompressedSubTitle);

        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestGetJournal()
        {
            const string documentNumberForJournal = "000175989"; //Newsweek
            var journal = (Journal)_repository.GetDocument(documentNumberForJournal, false);
            Assert.Equal("Journal", journal.GetType().Name);
            Assert.Equal("Newsweek", journal.Title);
            Assert.Equal("0163-7053", journal.Issn);
            Assert.Equal("the international newsmagazine", journal.SubTitle);
            Assert.Equal("Engelsk", journal.Language);
            Assert.Equal("51 nummer pr. år", journal.JournalsPerYear);
            Assert.Equal("Tidsskrift (19)", journal.CompressedSubTitle);

        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestGetCdPopular()
        {
            const string documentNumberForCdPopular = "000566205"; //Mods - Gje meg litt merr
            var cd = (Cd)_repository.GetDocument(documentNumberForCdPopular, false);
            Assert.Equal("Gje meg litt merr", cd.Title);
            Assert.Equal("Mods", cd.MusicGroup);
            Assert.Null(cd.ExplanatoryAddition);
            Assert.Equal("1 kompaktplate", cd.TypeAndNumberOfDiscs);
            //Assert.Equal("Innhold: Gje meg litt merr ; Belinda ; Revansj ; Me to går alltid aleina ; Amerika ; Bare i nått ; Eg e så forelska ; Ett år e gått ; Tore Tang ; Fint at du vil ; Eg vil hjem ; Hjelp meg ; Militæret ; Alexander ; Eg kom ikkje inn ; Regn ; Meg må du hilsa på ; Ikkje plag meg ; Bli med oss ; Livets roulette ; Another day ; Bahama Mama", cd.DiscContent);
            Assert.Equal("Utøvere: Kurt Ø. Olsen, Helge Hummervoll, Leif Nilsen, Morten A. Knutsen, Torkild Viig, Runar Bjaalid, Tor Øyvind Syvertsen", cd.Performers);
            Assert.Equal(2, cd.CompositionTypeOrGenre.Count());
            Assert.Equal("Popmusikk", cd.CompositionTypeOrGenre.ElementAt(0));
            Assert.Equal("Rock", cd.CompositionTypeOrGenre.ElementAt(1));
            Assert.Equal(7, cd.InvolvedPersons.Count());
            Assert.Empty(cd.InvolvedMusicGroups);
            Assert.Equal("Cd (2006)", cd.CompressedSubTitle);
        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestGetLanguageCourse()
        {
            const string documentNumberForLanguageCourse = "000391825"; //Jeg snakker norsk språkkurs
            var languageCourse = (LanguageCourse)_repository.GetDocument(documentNumberForLanguageCourse, false);
            Assert.Equal("Nils Ingnes", languageCourse.Author.Name);
            Assert.Equal("1941-", languageCourse.Author.LivingYears);
            Assert.Equal("Norsk", languageCourse.Author.Nationality);
            Assert.Equal("439.683", languageCourse.ClassificationNr);
            Assert.Equal(0, languageCourse.InvolvedOrganizations.Count());
            Assert.Equal(0, languageCourse.InvolvedPersons.Count());
            Assert.Equal("Engelsk", languageCourse.Language);
            Assert.Equal("Språkkurs", languageCourse.Subject.ElementAt(0));
            Assert.Equal("Norsk", languageCourse.Subject.ElementAt(1));
            Assert.Equal(2, languageCourse.Subject.Count());
            Assert.Equal(null, languageCourse.TitlesOtherWritingForms);
            Assert.Equal("4 CD plater og 1 veiledningshefte", languageCourse.TypeAndNumberOfDiscs);
            Assert.Equal("LanguageCourse", languageCourse.DocType);
            Assert.Equal("Språkkurs, Nils Ingnes (1999)", languageCourse.CompressedSubTitle);

        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestGetSheetMusic()
        {
            const string documentNumberForSheetMusic = "000117418"; //March (fanfare) for 3 trumpets and timpani
            var sheetMusic = (SheetMusic)_repository.GetDocument(documentNumberForSheetMusic, false);
            Assert.Equal("Carl Philipp Emanuel Bach", sheetMusic.Composer.Name);
            Assert.Equal("March (fanfare) for 3 trumpets and timpani", sheetMusic.Title);
            Assert.Equal("b. 4 st.", sheetMusic.NumberOfPagesAndNumberOfParts);
            Assert.Equal(2, sheetMusic.MusicalLineup.Count());
            Assert.Equal("Trompet 3", sheetMusic.MusicalLineup.ElementAt(0));
            Assert.Equal("Pauker", sheetMusic.MusicalLineup.ElementAt(1));
            Assert.Equal("Note", sheetMusic.CompressedSubTitle);

        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestGetNonExistingDoc()
        {
            const string documentNumberForBook = "abcdefg"; //Burde ikke funke
            var doc = _repository.GetDocument(documentNumberForBook, false);
            Assert.Null(doc);
        }

        [Fact]
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

            Assert.True(authUser.IsAuthorized);

        }

        [Fact]
        public void RequestReservationTest()
        {
            
            const string userId = "159222";
            const string docNumber = "000178569";
            const string branch = "Hovedbibl";

            var returnMessage = _repository.RequestReservation(docNumber, userId, branch);
            Assert.False(returnMessage.Success);

        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestDocumentLocation()
        {
            const string documentNumberForBook = "000596743"; //Naiv. Super
            var book = (Book)_repository.GetDocument(documentNumberForBook, false);
            Assert.Equal(2, book.AvailabilityInfo.Count());
            Assert.Equal("Hovedbibl.", book.AvailabilityInfo.ElementAt(0).Branch);
            Assert.Equal("Madla", book.AvailabilityInfo.ElementAt(1).Branch);
            //Assert.Equal("Kulturbiblioteket", book.AvailabilityInfo.ElementAt(0).Department);
            //Assert.Equal("Voksenavdelingen", book.AvailabilityInfo.ElementAt(1).Department);
            Assert.Equal("Skjønnlitteratur", book.AvailabilityInfo.ElementAt(0).PlacementCode);
            Assert.Equal("Skjønnlitteratur", book.AvailabilityInfo.ElementAt(1).PlacementCode);
        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestDocumentAvailability()
        {
            const string documentNumberForBook = "000596743"; //Naiv. Super
            var book = (Book)_repository.GetDocument(documentNumberForBook, false);
            Assert.Equal(1, book.AvailabilityInfo.ElementAt(0).AvailableCount);
            Assert.Equal(4, book.AvailabilityInfo.ElementAt(0).TotalCount);
            Assert.Equal(0, book.AvailabilityInfo.ElementAt(1).AvailableCount);
            Assert.Equal(1, book.AvailabilityInfo.ElementAt(1).TotalCount);
            Assert.NotNull(book.AvailabilityInfo.ElementAt(1).EarliestAvailableDateFormatted);
        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestNonAvailableDocument()
        {
            const string documentNumberForBook = "000611217"; //Naiv. Super
            var book = (Book)_repository.GetDocument(documentNumberForBook, false);
            Assert.Equal(0, book.AvailabilityInfo.ElementAt(0).AvailableCount);
            Assert.Equal(3, book.AvailabilityInfo.ElementAt(0).TotalCount);
            Assert.NotNull(book.AvailabilityInfo.ElementAt(0).EarliestAvailableDateFormatted);
        }

        [Fact(Skip = "Unnecessary integration test")]
        public void TestDocumentShouldOnlyIncludeOneBranch()
        {
            const string documentNumberForBook = "000530871"; //Hur kär får man bli?
            var book = (Book)_repository.GetDocument(documentNumberForBook, false);
            Assert.Equal(1, book.AvailabilityInfo.Count());
        }

    }

}
