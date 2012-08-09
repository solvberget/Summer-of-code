using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.DTOTests
{
    [TestFixture]
    public class TestModelConvertionFromXml
    {

        private readonly string _pathToRulesFolder = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\bin\App_Data\rules\");
        private IRulesRepository _repository;

        [SetUp]
        public void InitRepository()
        {
            _repository = new RulesRepository(_pathToRulesFolder);
        }
        
        [Test]
        public void GetDocumentFromXmlTest()
        {
            var media = Document.GetObjectFromFindDocXmlBsMarc(getBookXml());

            Assert.AreEqual("123456789", media.DocumentNumber);

            Assert.AreEqual("Voksne", media.TargetGroup);

            Assert.AreEqual("Fiksjon", media.IsFiction);

            Assert.AreEqual("Flerspråklig", media.Language);

            Assert.AreEqual("Norsk bokmål", media.Languages.ElementAt(0));
            Assert.AreEqual("Svensk", media.Languages.ElementAt(1));
            Assert.AreEqual("Engelsk", media.Languages.ElementAt(2));

            Assert.AreEqual("l", media.DocumentType.ElementAt(0));

            Assert.AreEqual("LOE", media.LocationCode);

            Assert.AreEqual("Naiv. Super", media.Title);

            Assert.AreEqual("Supert", media.SubTitle);

            Assert.AreEqual("Erlend Loe", media.ResponsiblePersons.ElementAt(0));

            Assert.AreEqual("[Oslo]", media.PlacePublished);

            Assert.AreEqual("Cappelen Damm", media.Publisher);

            Assert.AreEqual(2010, media.PublishedYear);

            Assert.AreEqual("Favoritt", media.SeriesTitle);

            Assert.AreEqual("5", media.SeriesNumber);

            Assert.AreEqual("Document", media.DocType);

        }

        [Test]
        public void GetDocumentLightFromXmlTest()
        {
            var media = Document.GetObjectFromFindDocXmlBsMarcLight(getBookXml());
            Assert.AreEqual("123456789", media.DocumentNumber);
            Assert.AreEqual("Flerspråklig", media.Language);
            Assert.AreEqual("l", media.DocumentType.ElementAt(0));
            Assert.AreEqual("Naiv. Super", media.Title);
            Assert.AreEqual(2010, media.PublishedYear);
            Assert.AreEqual("Document", media.DocType);
        }

        [Test]
        public void GetBookFromXmlTest()
        {
            var book = Book.GetObjectFromFindDocXmlBsMarc(getBookXml());

            Assert.AreEqual("978-82-02-33225-9", book.Isbn);

            Assert.AreEqual("598.0948", book.ClassificationNr);

            Assert.AreEqual("Erlend Loe", book.Author.Name);
            Assert.AreEqual("1969-", book.Author.LivingYears);
            Assert.AreEqual("n", book.Author.Nationality);

            Assert.AreEqual("Super Naiv", book.StdOrOrgTitle);

            Assert.AreEqual("5", book.Numbering);

            Assert.AreEqual("Tittel for del", book.PartTitle);

            Assert.AreEqual("5. oppl.", book.Edition);

            Assert.AreEqual("205 s.", book.NumberOfPages);

            Assert.AreEqual("Innhold: Et liv i fellesskap ; Bibelens bønnebok", book.Content);

            Assert.AreEqual("Kristendom", book.Subject.ElementAt(0));
            Assert.AreEqual("Buddhisme", book.Subject.ElementAt(1));
            Assert.AreEqual("Religionsvitenskap", book.Subject.ElementAt(2));

            Assert.AreEqual("Norge", book.ReferencedPlaces.ElementAt(0));
            Assert.AreEqual("Rogaland", book.ReferencedPlaces.ElementAt(1));

            Assert.AreEqual("Action", book.Genre.ElementAt(0));
            Assert.AreEqual("Komedie", book.Genre.ElementAt(1));

            Assert.AreEqual(1, book.ReferredPersons.Count());
            Assert.AreEqual("Knut Hamsun", book.ReferredPersons.ElementAt(0).Name);
            Assert.AreEqual("1859-1952", book.ReferredPersons.ElementAt(0).LivingYears);
            Assert.AreEqual("Norsk", book.ReferredPersons.ElementAt(0).Nationality);
            Assert.AreEqual("Sult", book.ReferredPersons.ElementAt(0).ReferredWork);

            Assert.AreEqual(1, book.ReferredOrganizations.Count());
            Assert.AreEqual("Capgemini", book.ReferredOrganizations.ElementAt(0).Name);
            Assert.AreEqual("B-tech", book.ReferredOrganizations.ElementAt(0).UnderOrganization);
            Assert.AreEqual("Forklaring", book.ReferredOrganizations.ElementAt(0).FurtherExplanation);
            Assert.AreEqual("Visjon", book.ReferredOrganizations.ElementAt(0).ReferencedPublication);

            Assert.AreEqual(2, book.InvolvedPersons.Count());
            Assert.AreEqual("Harald V", book.InvolvedPersons.ElementAt(0).Name);
            Assert.AreEqual("1937-", book.InvolvedPersons.ElementAt(0).LivingYears);
            Assert.AreEqual("Norsk", book.InvolvedPersons.ElementAt(0).Nationality);
            Assert.AreEqual("Konge", book.InvolvedPersons.ElementAt(0).Role);
            Assert.AreEqual("Kjell Inge Røkke", book.InvolvedPersons.ElementAt(1).Name);
            Assert.AreEqual("1950-", book.InvolvedPersons.ElementAt(1).LivingYears);
            Assert.AreEqual("Norsk", book.InvolvedPersons.ElementAt(1).Nationality);
            Assert.AreEqual("Investor", book.InvolvedPersons.ElementAt(1).Role);

            Assert.AreEqual(1, book.InvolvedOrganizations.Count());
            Assert.AreEqual("Cappelen Damm", book.InvolvedOrganizations.ElementAt(0).Name);
            Assert.AreEqual("Salg", book.InvolvedOrganizations.ElementAt(0).UnderOrganization);
            Assert.AreEqual("Forklaring", book.InvolvedOrganizations.ElementAt(0).FurtherExplanation);
            Assert.AreEqual("Forlag", book.InvolvedOrganizations.ElementAt(0).Role);

            Assert.AreEqual("Book", book.DocType);

            Assert.AreEqual("Cappelen Damm", book.Publisher);
        }

        [Test]
        public void GetBookLightFromXmlTest()
        {
            var book1 = Book.GetObjectFromFindDocXmlBsMarcLight(getBookXml());
            Assert.AreEqual("Erlend Loe", book1.Author.Name);
            Assert.AreEqual("1969-", book1.Author.LivingYears);
            Assert.AreEqual("n", book1.Author.Nationality);
            Assert.AreEqual("Book", book1.DocType);

            var book2 = Book.GetObjectFromFindDocXmlBsMarcLight(getBookWithOrgXml());
            Assert.AreEqual("Røde Kors", book2.Author.Name);
            Assert.AreEqual("Røde Kors", book2.Organization.Name);
            Assert.AreEqual("Hjelpekorpset", book2.Organization.UnderOrganization);
            Assert.AreEqual("Hjelper folk", book2.Organization.FurtherExplanation);

            var book3 = Book.GetObjectFromFindDocXmlBsMarcLight(getBookWithStdTitleXml());
            Assert.AreEqual("Røde Kors", book3.StandarizedTitle);

        }

        [Test]
        public void GetBookWithOrganizationFromXmlTest()
        {
            var book = Book.GetObjectFromFindDocXmlBsMarc(getBookWithOrgXml());

            Assert.AreEqual("Røde Kors", book.Author.Name);
            Assert.AreEqual("Røde Kors", book.Organization.Name);
            Assert.AreEqual("Hjelpekorpset", book.Organization.UnderOrganization);
            Assert.AreEqual("Hjelper folk", book.Organization.FurtherExplanation);

        }

        [Test]
        public void GetBookWithStdTitleFromXmlTest()
        {
            var book = Book.GetObjectFromFindDocXmlBsMarc(getBookWithStdTitleXml());
            Assert.AreEqual("Røde Kors", book.StandarizedTitle);
        }

        [Test]
        public void GetFilmFromXmlTest()
        {
            var film = Film.GetObjectFromFindDocXmlBsMarc(getFilmXml());

            Assert.AreEqual("Fakta", film.IsFiction);

            Assert.AreEqual("Barn og ungdom", film.TargetGroup);

            Assert.AreEqual("7041271735935", film.Ean);

            Assert.AreEqual("Norsk bokmål", film.SubtitleLanguage.FirstOrDefault());
            Assert.AreEqual(4, film.SubtitleLanguage.Count());

            Assert.AreEqual("Max Manus", film.OriginalTitle);

            Assert.AreEqual("1", film.Numbering);

            Assert.AreEqual("Max", film.PartTitle);

            Assert.AreEqual("Collector's edition", film.Edition);

            Assert.AreEqual("2008", film.ProductionYear);

            Assert.AreEqual("2 videoplater (DVD-video)(1 t 53 min)", film.TypeAndNumberOfDiscs);

            Assert.AreEqual("Innhold: Lily's theme ; Statues ; Neville the hero ; Courtyard apocalypse ; Severus and Lily ; Harry's sacrifice ; The resurrection stone ; A new beginning ; Lily's lullaby", film.Contents);

            Assert.AreEqual("Aksel Hennie", film.Actors.ToList()[0].Name);

            Assert.AreEqual("Aldersgrense: 15 år", film.AgeLimit);

            Assert.AreEqual("Max Manus", film.NorwegianTitle);

            Assert.AreEqual("Undervisning", film.Subject);

            Assert.AreEqual("Norge", film.ReferencedPlaces.ElementAt(0));

            Assert.AreEqual("Popmusikk", film.CompositionType);

            Assert.AreEqual("Drama", film.Genre.ElementAt(0));

            Assert.AreEqual(1, film.ReferredPersons.Count());
            Assert.AreEqual("Knut Hamsun", film.ReferredPersons.ElementAt(0).Name);
            Assert.AreEqual("1859-1952", film.ReferredPersons.ElementAt(0).LivingYears);
            Assert.AreEqual("Norsk", film.ReferredPersons.ElementAt(0).Nationality);
            Assert.AreEqual("Sult", film.ReferredPersons.ElementAt(0).ReferredWork);

            Assert.AreEqual(1, film.ReferredOrganizations.Count());
            Assert.AreEqual("Capgemini", film.ReferredOrganizations.ElementAt(0).Name);
            Assert.AreEqual("B-tech", film.ReferredOrganizations.ElementAt(0).UnderOrganization);
            Assert.AreEqual("Forklaring", film.ReferredOrganizations.ElementAt(0).FurtherExplanation);
            Assert.AreEqual("Visjon", film.ReferredOrganizations.ElementAt(0).ReferencedPublication);

            Assert.AreEqual(2, film.InvolvedPersons.Count());
            Assert.AreEqual("Espen Sandberg", film.InvolvedPersons.ElementAt(0).Name);
            Assert.AreEqual("1971-", film.InvolvedPersons.ElementAt(0).LivingYears);
            Assert.AreEqual("Norsk", film.InvolvedPersons.ElementAt(0).Nationality);
            Assert.AreEqual("regissør", film.InvolvedPersons.ElementAt(0).Role);
            Assert.AreEqual("Joachim Rønning", film.InvolvedPersons.ElementAt(1).Name);
            Assert.AreEqual("1972-", film.InvolvedPersons.ElementAt(1).LivingYears);
            Assert.AreEqual("Norsk", film.InvolvedPersons.ElementAt(1).Nationality);
            Assert.AreEqual("regissør", film.InvolvedPersons.ElementAt(1).Role);

            Assert.AreEqual(1, film.InvolvedOrganizations.Count());
            Assert.AreEqual("Cappelen Damm", film.InvolvedOrganizations.ElementAt(0).Name);
            Assert.AreEqual("Salg", film.InvolvedOrganizations.ElementAt(0).UnderOrganization);
            Assert.AreEqual("Forklaring", film.InvolvedOrganizations.ElementAt(0).FurtherExplanation);
            Assert.AreEqual("Forlag", film.InvolvedOrganizations.ElementAt(0).Role);

            Assert.AreEqual("Film", film.DocType);

        }

        [Test]
        public void GetFilmLightFromXmlTest()
        {
            var film = Film.GetObjectFromFindDocXmlBsMarcLight(getFilmXml());
            Assert.AreEqual("Max Manus", film.OriginalTitle);
            Assert.AreEqual("2008", film.ProductionYear);
            Assert.AreEqual("Aldersgrense: 15 år", film.AgeLimit);
            Assert.AreEqual("Drama", film.Genre.ElementAt(0));
            Assert.AreEqual("Film", film.DocType);
        }

        [Test]
        public void GetDocumentTypeFromXml()
        {
            var film = Film.GetObjectFromFindDocXmlBsMarc(getFilmXml());
            Assert.AreEqual("ee", film.DocumentType.ElementAt(0));

        }

        [Test]
        public void GetAudioBookFromXmlTest()
        {
            var audioBook = AudioBook.GetObjectFromFindDocXmlBsMarc(getAudioBookXML());

            Assert.AreEqual("978-82-02-29195-2", audioBook.Isbn);

            Assert.AreEqual("n781.542", audioBook.ClassificationNumber);

            Assert.AreEqual("J.K. Rowling", audioBook.Author.Name);

            Assert.AreEqual("1965-", audioBook.Author.LivingYears);

            Assert.AreEqual("Engelsk", audioBook.Author.Nationality);

            Assert.AreEqual("III", audioBook.Numbering);

            Assert.AreEqual("Atter en konge", audioBook.PartTitle);

            Assert.AreEqual("Collector's edition", audioBook.Edition);

            Assert.AreEqual("2 CDer (24 t, 35 min)", audioBook.TypeAndNumberOfDiscs);

            Assert.AreEqual("Harry Potter", audioBook.Subject);

            Assert.AreEqual("England", audioBook.ReferencedPlaces.ElementAt(0));

            Assert.AreEqual("Fantasy", audioBook.Genre.ElementAt(0));

            Assert.AreEqual(1, audioBook.ReferredPersons.Count());
            Assert.AreEqual("Knut Hamsun", audioBook.ReferredPersons.ElementAt(0).Name);
            Assert.AreEqual("1859-1952", audioBook.ReferredPersons.ElementAt(0).LivingYears);
            Assert.AreEqual("Norsk", audioBook.ReferredPersons.ElementAt(0).Nationality);
            Assert.AreEqual("Sult", audioBook.ReferredPersons.ElementAt(0).ReferredWork);

            Assert.AreEqual(1, audioBook.ReferredOrganizations.Count());
            Assert.AreEqual("Capgemini", audioBook.ReferredOrganizations.ElementAt(0).Name);
            Assert.AreEqual("B-tech", audioBook.ReferredOrganizations.ElementAt(0).UnderOrganization);
            Assert.AreEqual("Forklaring", audioBook.ReferredOrganizations.ElementAt(0).FurtherExplanation);
            Assert.AreEqual("Visjon", audioBook.ReferredOrganizations.ElementAt(0).ReferencedPublication);

            Assert.AreEqual(2, audioBook.InvolvedPersons.Count());
            Assert.AreEqual("Harald V", audioBook.InvolvedPersons.ElementAt(0).Name);
            Assert.AreEqual("1937-", audioBook.InvolvedPersons.ElementAt(0).LivingYears);
            Assert.AreEqual("Norsk", audioBook.InvolvedPersons.ElementAt(0).Nationality);
            Assert.AreEqual("Konge", audioBook.InvolvedPersons.ElementAt(0).Role);
            Assert.AreEqual("Kjell Inge Røkke", audioBook.InvolvedPersons.ElementAt(1).Name);
            Assert.AreEqual("1950-", audioBook.InvolvedPersons.ElementAt(1).LivingYears);
            Assert.AreEqual("Norsk", audioBook.InvolvedPersons.ElementAt(1).Nationality);
            Assert.AreEqual("Investor", audioBook.InvolvedPersons.ElementAt(1).Role);

            Assert.AreEqual(1, audioBook.InvolvedOrganizations.Count());
            Assert.AreEqual("Cappelen Damm", audioBook.InvolvedOrganizations.ElementAt(0).Name);
            Assert.AreEqual("Salg", audioBook.InvolvedOrganizations.ElementAt(0).UnderOrganization);
            Assert.AreEqual("Forklaring", audioBook.InvolvedOrganizations.ElementAt(0).FurtherExplanation);
            Assert.AreEqual("Forlag", audioBook.InvolvedOrganizations.ElementAt(0).Role);

            Assert.AreEqual("AudioBook", audioBook.DocType);


        }

        [Test]
        public void GetAudioBookLightFromXmlTest()
        {
            var audioBook1 = AudioBook.GetObjectFromFindDocXmlBsMarcLight(getAudioBookXML());
            Assert.AreEqual("J.K. Rowling", audioBook1.Author.Name);
            Assert.AreEqual("1965-", audioBook1.Author.LivingYears);
            Assert.AreEqual("Engelsk", audioBook1.Author.Nationality);
            Assert.AreEqual("AudioBook", audioBook1.DocType);

            var audioBook2 = AudioBook.GetObjectFromFindDocXmlBsMarcLight(getAudioBookWithOrgXml());
            Assert.AreEqual("Røde Kors", audioBook2.Author.Name);
            Assert.AreEqual("Røde Kors", audioBook2.Organization.Name);
            Assert.AreEqual("Hjelpekorpset", audioBook2.Organization.UnderOrganization);
            Assert.AreEqual("Hjelper folk", audioBook2.Organization.FurtherExplanation);

            var audioBook3 = AudioBook.GetObjectFromFindDocXmlBsMarcLight(getAudioBookWithStdTitleXml());
            Assert.AreEqual("Røde Kors", audioBook3.StandarizedTitle);

        }

        [Test]
        public void GetAudioBookWithOrganizationFromXmlTest()
        {
            var audioBook = AudioBook.GetObjectFromFindDocXmlBsMarc(getAudioBookWithOrgXml());
            Assert.AreEqual("Røde Kors", audioBook.Author.Name);
            Assert.AreEqual("Røde Kors", audioBook.Organization.Name);
            Assert.AreEqual("Hjelpekorpset", audioBook.Organization.UnderOrganization);
            Assert.AreEqual("Hjelper folk", audioBook.Organization.FurtherExplanation);
        }

        [Test]
        public void GetAudioBookWithStdTitleFromXmlTest()
        {
            var audioBook = AudioBook.GetObjectFromFindDocXmlBsMarc(getAudioBookWithStdTitleXml());
            Assert.AreEqual("Røde Kors", audioBook.StandarizedTitle);
        }

        [Test]
        public void GetJournalFromXmlTest()
        {
            var journal = Journal.GetObjectFromFindDocXmlBsMarc(getJournalXml());
            Assert.AreEqual("0163-7053", journal.Issn);
            Assert.AreEqual("51 nummer pr. år", journal.JournalsPerYear);
            Assert.AreEqual("Biblioteket har: Siste årg.", journal.InventoryInfomation);
            Assert.AreEqual(2, journal.Subject.Count());
            Assert.AreEqual("Nyheter", journal.Subject.ElementAt(0));
            Assert.AreEqual("Tidsskrifter", journal.Subject.ElementAt(1));
            Assert.IsEmpty(journal.ReferencedPlaces);
            Assert.AreEqual(0, journal.InvolvedPersons.Count());
            Assert.AreEqual(0, journal.InvolvedOrganizations.Count());
            Assert.AreEqual(0, journal.OtherTitles.Count());
        }

        [Test]
        public void GetJournalLightFromXmlTest()
        {
            var journal = Journal.GetObjectFromFindDocXmlBsMarcLight(getJournalXml());
            Assert.AreEqual("0163-7053", journal.Issn);
        }

        [Test]
        public void GetCdPopularFromXmlTest()
        {
            var cd = Cd.GetObjectFromFindDocXmlBsMarc(getCdPopularMusicGroupXml());
            Assert.AreEqual("Gje meg litt merr", cd.Title);
            Assert.AreEqual("Mods", cd.MusicGroup);
            Assert.IsNull(cd.ExplanatoryAddition);
            Assert.AreEqual("1 kompaktplate", cd.TypeAndNumberOfDiscs);
            Assert.AreEqual("Gje meg litt merr", cd.DiscContent.ElementAt(0));
            Assert.AreEqual("Utøvere: Kurt Ø. Olsen, Helge Hummervoll, Leif Nilsen, Morten A. Knutsen, Torkild Viig, Runar Bjaalid, Tor Øyvind Syvertsen", cd.Performers);
            Assert.AreEqual(2, cd.CompositionTypeOrGenre.Count());
            Assert.AreEqual("Popmusikk", cd.CompositionTypeOrGenre.ElementAt(0));
            Assert.AreEqual("Rock", cd.CompositionTypeOrGenre.ElementAt(1));
            Assert.AreEqual(7, cd.InvolvedPersons.Count());
            Assert.IsEmpty(cd.InvolvedMusicGroups);
        }

        [Test]
        public void GetCdPopularLightFromXmlTest()
        {
            var cd1 = Cd.GetObjectFromFindDocXmlBsMarcLight(getCdPopularMusicGroupXml());
            Assert.AreEqual("Mods", cd1.MusicGroup);

            var cd2 = Cd.GetObjectFromFindDocXmlBsMarcLight(getCdPopluarArtistXml());
            Assert.AreEqual("Morten Abel", cd2.ArtistOrComposer.Name);
        }

        [Test]
        public void GetLanguageCourseFromXmlTest()
        {
            var languageCourse = LanguageCourse.GetObjectFromFindDocXmlBsMarc(getLanguageCourseXml()); ;

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



            var languageCourse2 = LanguageCourse.GetObjectFromFindDocXmlBsMarc(getLanguageCourseWithIsbnXml());

            Assert.AreEqual("Margaretha Danbolt Simons", languageCourse2.Author.Name);

            Assert.AreEqual("439.8283", languageCourse2.ClassificationNr);

            Assert.AreEqual(0, languageCourse2.InvolvedPersons.Count());
            Assert.AreEqual(0, languageCourse2.InvolvedOrganizations.Count());

            Assert.AreEqual("978-1-444-10720-3", languageCourse2.Isbn);

            Assert.AreEqual("Engelsk", languageCourse.Language);

            Assert.AreEqual("Norsk", languageCourse2.Subject.ElementAt(0));
            Assert.AreEqual("Engelsk", languageCourse2.Subject.ElementAt(1));
            Assert.AreEqual("Språkkurs", languageCourse2.Subject.ElementAt(2));

            Assert.AreEqual(3, languageCourse2.Subject.Count());

            Assert.AreEqual(null, languageCourse2.TitlesOtherWritingForms);

            Assert.AreEqual("1 lærebok, 2 CD-er", languageCourse2.TypeAndNumberOfDiscs);

            Assert.AreEqual("LanguageCourse", languageCourse2.DocType);



            var languageCourse3 = LanguageCourse.GetObjectFromFindDocXmlBsMarc(getLanguageCourseWithInvolvedPersonsXml());
            Assert.AreEqual("8250804716", languageCourse3.Isbn);

            Assert.AreEqual("428.3", languageCourse3.ClassificationNr);

            Assert.AreEqual("Brenda Bennett", languageCourse3.Author.Name);




            Assert.AreEqual(1, languageCourse3.InvolvedPersons.Count());
            Assert.AreEqual("Diana Webster", languageCourse3.InvolvedPersons.ElementAt(0).Name);
            Assert.IsNull(languageCourse3.InvolvedPersons.ElementAt(0).LivingYears);

            Assert.AreEqual(3, languageCourse3.InvolvedOrganizations.Count());
            Assert.AreEqual("Norsk rikskringkasting", languageCourse3.InvolvedOrganizations.ElementAt(0).Name);
            Assert.AreEqual("Folkets brevskole", languageCourse3.InvolvedOrganizations.ElementAt(1).Name);
            Assert.AreEqual("NKS høgskole", languageCourse3.InvolvedOrganizations.ElementAt(2).Name);
            Assert.AreEqual("utgiver", languageCourse3.InvolvedOrganizations.ElementAt(0).Role);
            Assert.AreEqual("LanguageCourse", languageCourse3.DocType);
        }

        [Test]
        public void GetLanguageCourseLightFromXmlTest()
        {
            var languageCourse = LanguageCourse.GetObjectFromFindDocXmlBsMarcLight(getLanguageCourseWithIsbnXml());
            Assert.AreEqual("Margaretha Danbolt Simons", languageCourse.Author.Name);
        }

        [Test]
        public void GetSheetMusicFromXmlTest()
        {
            var sheetMusic = SheetMusic.GetObjectFromFindDocXmlBsMarc(getSheetMusicXml());
            Assert.AreEqual("Carl Philipp Emanuel Bach", sheetMusic.Composer.Name);
            Assert.AreEqual("March (fanfare) for 3 trumpets and timpani", sheetMusic.Title);
            Assert.AreEqual("b. 4 st.", sheetMusic.NumberOfPagesAndNumberOfParts);
            Assert.AreEqual(2, sheetMusic.MusicalLineup.Count());
            Assert.AreEqual("Trompet 3", sheetMusic.MusicalLineup.ElementAt(0));
            Assert.AreEqual("Pauker", sheetMusic.MusicalLineup.ElementAt(1));
        }

        [Test]
        public void GetSheetMusicLightFromXmlTest()
        {
            var sheetMuisc = SheetMusic.GetObjectFromFindDocXmlBsMarcLight(getSheetMusicXml());
            Assert.AreEqual("Carl Philipp Emanuel Bach", sheetMuisc.Composer.Name);
        }

        [Test]
        public void GetDocumentItemsFromXmlTest()
        {
            var documentItems = DocumentItem.GetDocumentItemsFromXml(getDocumentItemsXml(), getDocumentCircItemsXml(), _repository).ToList();
            Assert.AreEqual(6, documentItems.Count());

            var documentItem1 = documentItems.ElementAt(0);
            Assert.AreEqual("000611167000110", documentItem1.ItemKey);
            Assert.AreEqual("Hovedbibl.", documentItem1.Branch);
            Assert.AreEqual("Kulturbiblioteket", documentItem1.Department);
            Assert.AreEqual("04", documentItem1.ItemStatus);
            Assert.AreEqual("Skjønnlitteratur", documentItem1.PlacementCode);
            Assert.IsFalse(documentItem1.OnHold);
            Assert.IsNull(documentItem1.LoanStatus);
            Assert.IsFalse(documentItem1.InTransit);
            Assert.IsNull(documentItem1.LoanDueDate);

            var documentItem2 = documentItems.ElementAt(3);
            Assert.AreEqual("000611167000180", documentItem2.ItemKey);
            Assert.AreEqual("Hovedbibl.", documentItem2.Branch);
            Assert.AreEqual("Kulturbiblioteket", documentItem2.Department);
            Assert.AreEqual("04", documentItem2.ItemStatus);
            Assert.AreEqual("Skjønnlitteratur", documentItem2.PlacementCode);
            Assert.IsFalse(documentItem2.OnHold);
            Assert.AreEqual("A", documentItem2.LoanStatus);
            Assert.IsFalse(documentItem2.InTransit);
            //Assert.AreEqual("13.08.2012 00:00:00", documentItem2.LoanDueDate.ToString());
        }

        private string getLanguageCourseWithInvolvedPersonsXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<find-doc>
    <record>
        <metadata>
            <oai_marc>
                <fixfield id=""FMT"">BK</fixfield>
                <fixfield id=""LDR"">00948cim^^22003131^^45^^</fixfield>
                <fixfield id=""BAS"">25</fixfield>
                <fixfield id=""007"">ss</fixfield>
                <fixfield id=""008"">860228^1985^^^^^^^^^^^a^^^^^^^^^^0^eng^^</fixfield>
                <varfield id=""015"" i1="" "" i2="" "">
                    <subfield label=""a"">0146277</subfield>
                    <subfield label=""b"">media-f</subfield>
                </varfield>
                <varfield id=""019"" i1="" "" i2="" "">
                    <subfield label=""b"">db, dh</subfield>
                </varfield>
                <varfield id=""020"" i1="" "" i2="" "">
                    <subfield label=""a"">8250804716</subfield>
                </varfield>
                <varfield id=""041"" i1="" "" i2="" "">
                    <subfield label=""a"">eng</subfield>
                </varfield>
                <varfield id=""082"" i1="" "" i2="" "">
                    <subfield label=""a"">428.3</subfield>
                    <subfield label=""z"">h</subfield>
                </varfield>
                <varfield id=""090"" i1="" "" i2="" "">
                    <subfield label=""c"">428.3</subfield>
                    <subfield label=""d"">BEN</subfield>
                </varfield>
                <varfield id=""091"" i1="" "" i2="" "">
                    <subfield label=""f"">0</subfield>
                    <subfield label=""h"">eng</subfield>
                </varfield>
                <varfield id=""100"" i1="" "" i2="" "">
                    <subfield label=""a"">Bennett, Brenda</subfield>
                </varfield>
                <varfield id=""245"" i1="" "" i2="" "">
                    <subfield label=""a"">Take it easy 4 : lydopptak : A-b og C kassetter: radioprogrammene: kassett 1-9</subfield>
                </varfield>
                <varfield id=""260"" i1="" "" i2="" "">
                    <subfield label=""a"">Oslo</subfield>
                    <subfield label=""b"">Folkets brevskole</subfield>
                    <subfield label=""c"">1985</subfield>
                </varfield>
                <varfield id=""300"" i1="" "" i2="" "">
                    <subfield label=""b"">koffert (9 kassetter, 1 bok)</subfield>
                </varfield>
                <varfield id=""500"" i1="" "" i2="" "">
                    <subfield label=""a"">Verket består av flere bind</subfield>
                </varfield>
                <varfield id=""500"" i1="" "" i2="" "">
                    <subfield label=""a"">Eget teksthefte til kassettene</subfield>
                </varfield>
                <varfield id=""599"" i1="" "" i2="" "">
                    <subfield label=""a"">0570</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Engelsk</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Språkkurs</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Lærebøker</subfield>
                </varfield>
                <varfield id=""700"" i1="" "" i2="" "">
                    <subfield label=""a"">Webster, Diana</subfield>
                </varfield>
                <varfield id=""710"" i1="" "" i2="" "">
                    <subfield label=""a"">Norsk rikskringkasting</subfield>
                    <subfield label=""e"">utgiver</subfield>
                </varfield>
                <varfield id=""710"" i1="" "" i2="" "">
                    <subfield label=""a"">Folkets brevskole</subfield>
                    <subfield label=""e"">utgiver</subfield>
                </varfield>
                <varfield id=""710"" i1="" "" i2="" "">
                    <subfield label=""a"">NKS høgskole</subfield>
                </varfield>
                <varfield id=""850"" i1="" "" i2="" "">
                    <subfield label=""a"">stavangb</subfield>
                    <subfield label=""c"">428.3 BEN</subfield>
                    <subfield label=""d"">1985</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20041103</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1340</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b""></subfield>
                    <subfield label=""c"">19971213</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1805</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b""></subfield>
                    <subfield label=""c"">19990531</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1512</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b""></subfield>
                    <subfield label=""c"">19991116</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">0954</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20000409</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1241</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20000704</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1240</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20001212</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1751</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20010225</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1633</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20010305</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1221</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20020217</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1604</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20020519</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">0915</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20020520</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">0854</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20020708</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1556</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20020708</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1557</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20020715</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1920</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20021102</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">2307</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20021104</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">0524</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20021119</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1523</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20041104</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1255</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20050531</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1353</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20060818</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1153</subfield>
                </varfield>
            </oai_marc>
        </metadata>
    </record>
    <session-id>KEM8UE8U3SXKJSTU8HHND7CY589KGLBXBSS9CD94FQHFR3AUV5</session-id>
</find-doc>";
        }

        private string getLanguageCourseWithIsbnXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<find-doc>
    <record>
        <metadata>
            <oai_marc>
                <fixfield id=""FMT"">MX</fixfield>
                <fixfield id=""LDR"">^^^^^nom^^^^^^^^^1</fixfield>
                <fixfield id=""008"">110121^2010^^^^^^^^^^^a^^^^^^^^^^0^mul^2</fixfield>
                <varfield id=""019"" i1="" "" i2="" "">
                    <subfield label=""b"">dk,dh</subfield>
                    <subfield label=""d"">L</subfield>
                </varfield>
                <varfield id=""020"" i1="" "" i2="" "">
                    <subfield label=""a"">978-1-444-10720-3</subfield>
                </varfield>
                <varfield id=""041"" i1="" "" i2="" "">
                    <subfield label=""a"">engnob</subfield>
                </varfield>
                <varfield id=""082"" i1="" "" i2="" "">
                    <subfield label=""a"">439.8283</subfield>
                    <subfield label=""z"">h</subfield>
                </varfield>
                <varfield id=""090"" i1="" "" i2="" "">
                    <subfield label=""c"">439.8283</subfield>
                    <subfield label=""d"">SIM</subfield>
                </varfield>
                <varfield id=""100"" i1="" "" i2=""0"">
                    <subfield label=""a"">Simons, Margaretha Danbolt</subfield>
                </varfield>
                <varfield id=""245"" i1=""1"" i2=""0"">
                    <subfield label=""a"">Complete Norwegian</subfield>
                    <subfield label=""h"">kombidokument</subfield>
                    <subfield label=""c"">Margaretha Danbolt Simons</subfield>
                </varfield>
                <varfield id=""260"" i1="" "" i2="" "">
                    <subfield label=""a"">London</subfield>
                    <subfield label=""b"">Hodder Education</subfield>
                    <subfield label=""c"">2010</subfield>
                </varfield>
                <varfield id=""300"" i1="" "" i2="" "">
                    <subfield label=""a"">1 lærebok, 2 CD-er</subfield>
                </varfield>
                <varfield id=""440"" i1="" "" i2=""0"">
                    <subfield label=""a"">Teach yourself</subfield>
                    <subfield label=""p"">Level 4</subfield>
                </varfield>
                <varfield id=""546"" i1="" "" i2="" "">
                    <subfield label=""a"">Tekst og tale på engelsk og norsk</subfield>
                </varfield>
                <varfield id=""599"" i1="" "" i2="" "">
                    <subfield label=""a"">450 kr</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Norsk</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Engelsk</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Språkkurs</subfield>
                </varfield>
                <varfield id=""850"" i1="" "" i2="" "">
                    <subfield label=""a"">stavangb</subfield>
                    <subfield label=""c"">439.8283 SIM</subfield>
                    <subfield label=""d"">2010</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BSONLINE</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20110121</subfield>
                    <subfield label=""l"">nor01</subfield>
                    <subfield label=""h"">0911</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">KATALOG</subfield>
                    <subfield label=""b"">40</subfield>
                    <subfield label=""c"">20110218</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">0947</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">KATALOG</subfield>
                    <subfield label=""b"">40</subfield>
                    <subfield label=""c"">20110218</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">0947</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">KATALOG</subfield>
                    <subfield label=""b"">40</subfield>
                    <subfield label=""c"">20110321</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1249</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">KATALOG</subfield>
                    <subfield label=""b"">40</subfield>
                    <subfield label=""c"">20110321</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1250</subfield>
                </varfield>
            </oai_marc>
        </metadata>
    </record>
    <session-id>GX9BGIHB3C3E7SS4NCUGSLDE48Y6MJYVYLKVG258RHI2MBMMTS</session-id>
</find-doc>";
        }

        private string getLanguageCourseXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<find-doc>
    <record>
        <metadata>
            <oai_marc>
                <fixfield id=""FMT"">BK</fixfield>
                <fixfield id=""LDR"">^^^^^nom^^^^^^^^^1</fixfield>
                <fixfield id=""BAS"">25</fixfield>
                <fixfield id=""007"">sc</fixfield>
                <fixfield id=""008"">990408^1999^^^^^^^j^^^a^^^^^^^^^^0^eng^</fixfield>
                <varfield id=""019"" i1="" "" i2="" "">
                    <subfield label=""b"">dc, dh</subfield>
                </varfield>
                <varfield id=""041"" i1="" "" i2="" "">
                    <subfield label=""a"">nob</subfield>
                </varfield>
                <varfield id=""082"" i1="" "" i2="" "">
                    <subfield label=""a"">439.683</subfield>
                    <subfield label=""z"">h</subfield>
                </varfield>
                <varfield id=""090"" i1="" "" i2="" "">
                    <subfield label=""c"">439.683</subfield>
                    <subfield label=""d"">ING</subfield>
                </varfield>
                <varfield id=""100"" i1="" "" i2="" "">
                    <subfield label=""a"">Ingnes, Nils</subfield>
                    <subfield label=""d"">1941-</subfield>
                    <subfield label=""j"">n.</subfield>
                </varfield>
                <varfield id=""245"" i1=""1"" i2="" "">
                    <subfield label=""a"">Jeg snakker norsk : norsk-engelsk</subfield>
                    <subfield label=""b"">course in practical Norwegian : level: beginners/intermediate</subfield>
                </varfield>
                <varfield id=""260"" i1="" "" i2="" "">
                    <subfield label=""a"">Elverum</subfield>
                    <subfield label=""b"">Språkconcult</subfield>
                    <subfield label=""c"">[1999]</subfield>
                </varfield>
                <varfield id=""300"" i1="" "" i2="" "">
                    <subfield label=""a"">4 CD plater og 1 veiledningshefte</subfield>
                </varfield>
                <varfield id=""440"" i1="" "" i2="" "">
                    <subfield label=""a"">Språkconsult&apos;s populærserie</subfield>
                </varfield>
                <varfield id=""599"" i1="" "" i2="" "">
                    <subfield label=""a"">1190 kr</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Språkkurs</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Norsk</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20041103</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1435</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b""></subfield>
                    <subfield label=""c"">19990531</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">2252</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">STVGBIBL</subfield>
                    <subfield label=""b"">10</subfield>
                    <subfield label=""c"">19990406</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1143</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">STVGBIBL</subfield>
                    <subfield label=""b"">10</subfield>
                    <subfield label=""c"">19990406</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1205</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">STVGBIBL</subfield>
                    <subfield label=""b"">10</subfield>
                    <subfield label=""c"">19990408</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1347</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">STVGBIBL</subfield>
                    <subfield label=""b"">10</subfield>
                    <subfield label=""c"">19990408</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1357</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">STVGBIBL</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20030115</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1252</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">STVGBIBL</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20040513</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1314</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">STVGBIBL</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20040513</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1315</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20041104</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1356</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20050531</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1454</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20060818</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1238</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH-UPD</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20110208</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1246</subfield>
                </varfield>
            </oai_marc>
        </metadata>
    </record>
    <session-id>G53JL4AYQLTJX4QNAFJHDN8YC3STBI1VG5G9SUQ44PA6IUHVQ3</session-id>
</find-doc>";
        }

        private string getBookXml()
        {
            return @"<find-doc>
  <record>
    <doc_number>123456789</doc_number>
    <metadata>
      <oai_marc>
        <fixfield id=""FMT"">BK</fixfield>
        <fixfield id=""LDR"">^^^^^nam^^^^^^^^^1</fixfield>
        <fixfield id=""008"">110106s2010^^^^^^^^^^^a^^^^^^^^^^1^mul^^</fixfield>
        <varfield id=""019"" i1="" "" i2="" "">
          <subfield label=""b"">l</subfield>
          <subfield label=""d"">R</subfield>
        </varfield>
        <varfield id=""020"" i1="" "" i2="" "">
          <subfield label=""a"">978-82-02-33225-9</subfield>
          <subfield label=""b"">ib.</subfield>
        </varfield>
        <varfield id=""041"" i1="" "" i2="" "">
          <subfield label=""a"">nobsweeng</subfield>
        </varfield>
        <varfield id=""090"" i1="" "" i2="" "">
          <subfield label=""c"">598.0948</subfield>
          <subfield label=""d"">LOE</subfield>
        </varfield>
        <varfield id=""100"" i1="" "" i2=""0"">
          <subfield label=""a"">Loe, Erlend</subfield>
          <subfield label=""d"">1969-</subfield>
          <subfield label=""j"">n</subfield>
        </varfield>
        <varfield id=""240"" i1="" "" i2="" "">
          <subfield label=""a"">Super Naiv</subfield>
        </varfield>
        <varfield id=""245"" i1=""1"" i2=""0"">
          <subfield label=""a"">Naiv. Super</subfield>
          <subfield label=""b"">Supert</subfield>
          <subfield label=""c"">Erlend Loe</subfield>
          <subfield label=""n"">5</subfield>
          <subfield label=""p"">Tittel for del</subfield>
        </varfield>
        <varfield id=""250"" i1="" "" i2="" "">
          <subfield label=""a"">5. oppl.</subfield>
        </varfield>
        <varfield id=""260"" i1="" "" i2="" "">
          <subfield label=""a"">[Oslo]</subfield>
          <subfield label=""b"">Cappelen Damm</subfield>
          <subfield label=""c"">2010</subfield>
        </varfield>
        <varfield id=""300"" i1="" "" i2="" "">
          <subfield label=""a"">205 s.</subfield>
        </varfield>
        <varfield id=""440"" i1="" "" i2=""0"">
          <subfield label=""a"">Favoritt</subfield>
          <subfield label=""v"">5</subfield>
        </varfield>
        <varfield id=""503"" i1="" "" i2="" "">
          <subfield label=""a"">1. utg.: Oslo : Cappelen, 1996</subfield>
        </varfield>
        <varfield id=""505"" i1="" "" i2="" "">
          <subfield label=""a"">Innhold: Et liv i fellesskap ; Bibelens bønnebok</subfield>
        </varfield>
        <varfield id=""599"" i1="" "" i2="" "">
          <subfield label=""a"">200 kr</subfield>
        </varfield>
        <varfield id=""600"" i1="" "" i2="" "">
          <subfield label=""a"">Knut Hamsun</subfield>
          <subfield label=""d"">1859-1952</subfield>
          <subfield label=""j"">Norsk</subfield>
          <subfield label=""t"">Sult</subfield>
        </varfield>
        <varfield id=""610"" i1="" "" i2="" "">
          <subfield label=""a"">Capgemini</subfield>
          <subfield label=""b"">B-tech</subfield>
          <subfield label=""q"">Forklaring</subfield>
          <subfield label=""t"">Visjon</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Kristendom</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Buddhisme</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Religionsvitenskap</subfield>
        </varfield>
        <varfield id=""651"" i1="" "" i2="" "">
          <subfield label=""a"">Norge</subfield>
        </varfield>
        <varfield id=""651"" i1="" "" i2="" "">
          <subfield label=""a"">Rogaland</subfield>
        </varfield>
        <varfield id=""655"" i1="" "" i2="" "">
          <subfield label=""a"">Action</subfield>
        </varfield>
        <varfield id=""655"" i1="" "" i2="" "">
          <subfield label=""a"">Komedie</subfield>
        </varfield>
        <varfield id=""700"" i1="" "" i2="" "">
          <subfield label=""a"">Harald V</subfield>
          <subfield label=""d"">1937-</subfield>
          <subfield label=""j"">Norsk</subfield>
          <subfield label=""e"">Konge</subfield>
        </varfield>
        <varfield id=""700"" i1="" "" i2="" "">
          <subfield label=""a"">Kjell Inge Røkke</subfield>
          <subfield label=""d"">1950-</subfield>
          <subfield label=""j"">Norsk</subfield>
          <subfield label=""e"">Investor</subfield>
        </varfield>
        <varfield id=""710"" i1="" "" i2="" "">
          <subfield label=""a"">Cappelen Damm</subfield>
          <subfield label=""b"">Salg</subfield>
          <subfield label=""e"">Forlag</subfield>
          <subfield label=""q"">Forklaring</subfield>
        </varfield>
        <varfield id=""850"" i1="" "" i2="" "">
          <subfield label=""a"">stavangb</subfield>
          <subfield label=""c"">LOE</subfield>
          <subfield label=""d"">2010</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">LHA</subfield>
          <subfield label=""b"">30</subfield>
          <subfield label=""c"">20110106</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1254</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">BATCH-UPD</subfield>
          <subfield label=""b"">30</subfield>
          <subfield label=""c"">20110106</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1254</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">KATALOG</subfield>
          <subfield label=""b"">40</subfield>
          <subfield label=""c"">20110201</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1136</subfield>
        </varfield>
      </oai_marc>
    </metadata>
  </record>
  <session-id>XXIT5PJTANKBX77H4PR6X8VJMN3BTGXFEURFSCSIH4FBMJSXHX</session-id>
</find-doc>";
        }

        private string getBookWithOrgXml()
        {
            return @"<find-doc>
  <record>
    <doc_number>123456789</doc_number>
    <metadata>
      <oai_marc>
        <fixfield id=""FMT"">BK</fixfield>
        <fixfield id=""LDR"">^^^^^nam^^^^^^^^^1</fixfield>
        <fixfield id=""008"">110106s2010^^^^^^^^^^^a^^^^^^^^^^1^mul^^</fixfield>
        <varfield id=""019"" i1="" "" i2="" "">
          <subfield label=""b"">l</subfield>
          <subfield label=""d"">R</subfield>
        </varfield>
        <varfield id=""020"" i1="" "" i2="" "">
          <subfield label=""a"">978-82-02-33225-9</subfield>
          <subfield label=""b"">ib.</subfield>
        </varfield>
        <varfield id=""041"" i1="" "" i2="" "">
          <subfield label=""a"">nobsweeng</subfield>
        </varfield>
        <varfield id=""090"" i1="" "" i2="" "">
          <subfield label=""c"">598.0948</subfield>
          <subfield label=""d"">LOE</subfield>
        </varfield>
        <varfield id=""110"" i1="" "" i2=""0"">
          <subfield label=""a"">Røde Kors</subfield>
          <subfield label=""b"">Hjelpekorpset</subfield>
          <subfield label=""q"">Hjelper folk</subfield>
        </varfield>
        <varfield id=""245"" i1=""1"" i2=""0"">
          <subfield label=""a"">Naiv. Super</subfield>
          <subfield label=""b"">Supert</subfield>
          <subfield label=""c"">Erlend Loe</subfield>
          <subfield label=""n"">5</subfield>
        </varfield>
        <varfield id=""250"" i1="" "" i2="" "">
          <subfield label=""a"">5. oppl.</subfield>
        </varfield>
        <varfield id=""260"" i1="" "" i2="" "">
          <subfield label=""a"">[Oslo]</subfield>
          <subfield label=""b"">Cappelen Damm</subfield>
          <subfield label=""c"">2010</subfield>
        </varfield>
        <varfield id=""300"" i1="" "" i2="" "">
          <subfield label=""a"">205 s.</subfield>
        </varfield>
        <varfield id=""440"" i1="" "" i2=""0"">
          <subfield label=""a"">Favoritt</subfield>
          <subfield label=""v"">5</subfield>
        </varfield>
        <varfield id=""503"" i1="" "" i2="" "">
          <subfield label=""a"">1. utg.: Oslo : Cappelen, 1996</subfield>
        </varfield>
        <varfield id=""505"" i1="" "" i2="" "">
          <subfield label=""a"">Innhold: Et liv i fellesskap ; Bibelens bønnebok</subfield>
        </varfield>
        <varfield id=""599"" i1="" "" i2="" "">
          <subfield label=""a"">200 kr</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Kristendom</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Buddhisme</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Religionsvitenskap</subfield>
        </varfield>
        <varfield id=""651"" i1="" "" i2="" "">
          <subfield label=""a"">Norge</subfield>
        </varfield>
        <varfield id=""651"" i1="" "" i2="" "">
          <subfield label=""a"">Rogaland</subfield>
        </varfield>
        <varfield id=""655"" i1="" "" i2="" "">
          <subfield label=""a"">Action</subfield>
        </varfield>
        <varfield id=""655"" i1="" "" i2="" "">
          <subfield label=""a"">Komedie</subfield>
        </varfield>
        <varfield id=""850"" i1="" "" i2="" "">
          <subfield label=""a"">stavangb</subfield>
          <subfield label=""c"">LOE</subfield>
          <subfield label=""d"">2010</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">LHA</subfield>
          <subfield label=""b"">30</subfield>
          <subfield label=""c"">20110106</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1254</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">BATCH-UPD</subfield>
          <subfield label=""b"">30</subfield>
          <subfield label=""c"">20110106</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1254</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">KATALOG</subfield>
          <subfield label=""b"">40</subfield>
          <subfield label=""c"">20110201</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1136</subfield>
        </varfield>
      </oai_marc>
    </metadata>
  </record>
  <session-id>XXIT5PJTANKBX77H4PR6X8VJMN3BTGXFEURFSCSIH4FBMJSXHX</session-id>
</find-doc>";
        }

        private string getBookWithStdTitleXml()
        {
            return @"<find-doc>
  <record>
    <doc_number>123456789</doc_number>
    <metadata>
      <oai_marc>
        <fixfield id=""FMT"">BK</fixfield>
        <fixfield id=""LDR"">^^^^^nam^^^^^^^^^1</fixfield>
        <fixfield id=""008"">110106s2010^^^^^^^^^^^a^^^^^^^^^^1^mul^^</fixfield>
        <varfield id=""019"" i1="" "" i2="" "">
          <subfield label=""b"">l</subfield>
          <subfield label=""d"">R</subfield>
        </varfield>
        <varfield id=""020"" i1="" "" i2="" "">
          <subfield label=""a"">978-82-02-33225-9</subfield>
          <subfield label=""b"">ib.</subfield>
        </varfield>
        <varfield id=""041"" i1="" "" i2="" "">
          <subfield label=""a"">nobsweeng</subfield>
        </varfield>
        <varfield id=""090"" i1="" "" i2="" "">
          <subfield label=""c"">598.0948</subfield>
          <subfield label=""d"">LOE</subfield>
        </varfield>
        <varfield id=""130"" i1="" "" i2=""0"">
          <subfield label=""a"">Røde Kors</subfield>
        </varfield>
        <varfield id=""245"" i1=""1"" i2=""0"">
          <subfield label=""a"">Naiv. Super</subfield>
          <subfield label=""b"">Supert</subfield>
          <subfield label=""c"">Erlend Loe</subfield>
          <subfield label=""n"">5</subfield>
        </varfield>
        <varfield id=""250"" i1="" "" i2="" "">
          <subfield label=""a"">5. oppl.</subfield>
        </varfield>
        <varfield id=""260"" i1="" "" i2="" "">
          <subfield label=""a"">[Oslo]</subfield>
          <subfield label=""b"">Cappelen Damm</subfield>
          <subfield label=""c"">2010</subfield>
        </varfield>
        <varfield id=""300"" i1="" "" i2="" "">
          <subfield label=""a"">205 s.</subfield>
        </varfield>
        <varfield id=""440"" i1="" "" i2=""0"">
          <subfield label=""a"">Favoritt</subfield>
          <subfield label=""v"">5</subfield>
        </varfield>
        <varfield id=""503"" i1="" "" i2="" "">
          <subfield label=""a"">1. utg.: Oslo : Cappelen, 1996</subfield>
        </varfield>
        <varfield id=""505"" i1="" "" i2="" "">
          <subfield label=""a"">Innhold: Et liv i fellesskap ; Bibelens bønnebok</subfield>
        </varfield>
        <varfield id=""599"" i1="" "" i2="" "">
          <subfield label=""a"">200 kr</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Kristendom</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Buddhisme</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Religionsvitenskap</subfield>
        </varfield>
        <varfield id=""651"" i1="" "" i2="" "">
          <subfield label=""a"">Norge</subfield>
        </varfield>
        <varfield id=""651"" i1="" "" i2="" "">
          <subfield label=""a"">Rogaland</subfield>
        </varfield>
        <varfield id=""655"" i1="" "" i2="" "">
          <subfield label=""a"">Action</subfield>
        </varfield>
        <varfield id=""655"" i1="" "" i2="" "">
          <subfield label=""a"">Komedie</subfield>
        </varfield>
        <varfield id=""850"" i1="" "" i2="" "">
          <subfield label=""a"">stavangb</subfield>
          <subfield label=""c"">LOE</subfield>
          <subfield label=""d"">2010</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">LHA</subfield>
          <subfield label=""b"">30</subfield>
          <subfield label=""c"">20110106</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1254</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">BATCH-UPD</subfield>
          <subfield label=""b"">30</subfield>
          <subfield label=""c"">20110106</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1254</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">KATALOG</subfield>
          <subfield label=""b"">40</subfield>
          <subfield label=""c"">20110201</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1136</subfield>
        </varfield>
      </oai_marc>
    </metadata>
  </record>
  <session-id>XXIT5PJTANKBX77H4PR6X8VJMN3BTGXFEURFSCSIH4FBMJSXHX</session-id>
</find-doc>";
        }

        private string getFilmXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<find-doc>
    <record>
        <doc_number>123456789</doc_number>
        <metadata>
            <oai_marc>
                <fixfield id=""FMT"">VM</fixfield>
                <fixfield id=""LDR"">^^^^^ngm^^^^^^^^^1</fixfield>
                <fixfield id=""007"">vd</fixfield>
                <fixfield id=""008"">090626s2009^^^^^^^^^^^j^^^^^^^^^^0^nob^^</fixfield>
                <varfield id=""019"" i1="" "" i2="" "">
                    <subfield label=""b"">ee</subfield>
                </varfield>
                <varfield id=""025"" i1="" "" i2="" "">
                    <subfield label=""a"">7041271735935</subfield>
                </varfield>
                <varfield id=""041"" i1="" "" i2="" "">
                    <subfield label=""a"">nob</subfield>
                    <subfield label=""b"">nobdanswefin</subfield>
                </varfield>
                <varfield id=""090"" i1="" "" i2="" "">
                    <subfield label=""d"">MAX</subfield>
                </varfield>
                <varfield id=""240"" i1="" "" i2="" "">
                    <subfield label=""a"">Max Manus</subfield>
                </varfield>
                <varfield id=""245"" i1=""0"" i2=""0"">
                    <subfield label=""a"">Max Manus</subfield>
                    <subfield label=""c"">regi: Espen Sandberg og Joachim Rønning ; manuskript Thomas Nordset-Tiller</subfield>
                    <subfield label=""h"">DVD</subfield>
                    <subfield label=""n"">1</subfield>
                    <subfield label=""p"">Max</subfield>
                </varfield>
                <varfield id=""250"" i1="" "" i2="" "">
                    <subfield label=""a"">Collector's edition</subfield>
                </varfield>
                <varfield id=""260"" i1="" "" i2="" "">
                    <subfield label=""a"">[Oslo]</subfield>
                    <subfield label=""b"">Nordisk film</subfield>
                    <subfield label=""c"">[2009]</subfield>
                    <subfield label=""g"">2008</subfield>
                </varfield>
                <varfield id=""300"" i1="" "" i2="" "">
                    <subfield label=""a"">2 videoplater (DVD-video)(1 t 53 min)</subfield>
                    <subfield label=""b"">lyd, kol.</subfield>
                </varfield>
                <varfield id=""503"" i1="" "" i2="" "">
                    <subfield label=""a"">Filmen ble laget i 2008</subfield>
                </varfield>
                <varfield id=""505"" i1="" "" i2="" "">
                    <subfield label=""a"">Innhold: Lily&apos;s theme ; Statues ; Neville the hero ; Courtyard apocalypse ; Severus and Lily ; Harry&apos;s sacrifice ; The resurrection stone ; A new beginning ; Lily&apos;s lullaby</subfield>
                </varfield>
                <varfield id=""511"" i1="" "" i2="" "">
                    <subfield label=""a"">Rolleliste: Aksel Hennie, Nicolai Cleve Broch, Christian Rubeck, Knut Joner, Mats Eldøen, Pål Sverre Valheim, Agnes Kittelsen, Viktoria Winge, Kyrre Haugen Sydness, Jakob Oftebro, Petter Næss</subfield>
                </varfield>
                <varfield id=""521"" i1="" "" i2="" "">
                    <subfield label=""a"">Aldersgrense: 15 år</subfield>
                </varfield>
                <varfield id=""546"" i1="" "" i2="" "">
                    <subfield label=""a"">Norsk tale, valgfri tekst på flere språk</subfield>
                </varfield>
                <varfield id=""599"" i1="" "" i2="" "">
                    <subfield label=""a"">200 kr</subfield>
                </varfield>
                <varfield id=""572"" i1="" "" i2="" "">
                    <subfield label=""a"">Max Manus</subfield>
                </varfield>
                <varfield id=""600"" i1="" "" i2="" "">
                  <subfield label=""a"">Knut Hamsun</subfield>
                  <subfield label=""d"">1859-1952</subfield>
                  <subfield label=""j"">Norsk</subfield>
                  <subfield label=""t"">Sult</subfield>
                </varfield>
                <varfield id=""610"" i1="" "" i2="" "">
                  <subfield label=""a"">Capgemini</subfield>
                  <subfield label=""b"">B-tech</subfield>
                  <subfield label=""q"">Forklaring</subfield>
                  <subfield label=""t"">Visjon</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Undervisning</subfield>
                </varfield>
                <varfield id=""651"" i1="" "" i2="" "">
                    <subfield label=""a"">Norge</subfield>
                    <subfield label=""q"">produksjonsland</subfield>
                </varfield>
                <varfield id=""652"" i1="" "" i2="" "">
                    <subfield label=""a"">Popmusikk</subfield>
                </varfield>
                <varfield id=""655"" i1="" "" i2="" "">
                    <subfield label=""a"">Drama</subfield>
                </varfield>
                <varfield id=""691"" i1="" "" i2="" "">
                    <subfield label=""a"">Krigen 1939-1945</subfield>
                </varfield>
                <varfield id=""691"" i1="" "" i2="" "">
                    <subfield label=""a"">Norge</subfield>
                </varfield>
                <varfield id=""700"" i1=""1"" i2=""0"">
                    <subfield label=""a"">Sandberg, Espen</subfield>
                    <subfield label=""d"">1971-</subfield>
                    <subfield label=""j"">n.</subfield>
                    <subfield label=""e"">regissør</subfield>
                </varfield>
                <varfield id=""700"" i1=""1"" i2=""0"">
                    <subfield label=""a"">Rønning, Joachim</subfield>
                    <subfield label=""d"">1972-</subfield>
                    <subfield label=""j"">n.</subfield>
                    <subfield label=""e"">regissør</subfield>
                </varfield>
                <varfield id=""710"" i1="" "" i2="" "">
                  <subfield label=""a"">Cappelen Damm</subfield>
                  <subfield label=""b"">Salg</subfield>
                  <subfield label=""e"">Forlag</subfield>
                  <subfield label=""q"">Forklaring</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">KATALOG</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20090626</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1051</subfield>
                </varfield>
            </oai_marc>
        </metadata>
    </record>
    <session-id>T5VLQNV2CIYLSGIXLXY5IE8Y8KASU29MCGA6685TEHJ5Y8DTDK</session-id>
</find-doc>
 ";
        }

        private string getAudioBookXML()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<find-doc>
    <record>
        <doc_number>123456789</doc_number>
        <metadata>
            <oai_marc>
                <fixfield id=""FMT"">BK</fixfield>
                <fixfield id=""LDR"">^^^^^nim^^^^^^^^^1</fixfield>
                <fixfield id=""008"">110324^2008^^^^^^^^^^^j^^^^^^^^^^1^nob^2</fixfield>
                <varfield id=""019"" i1="" "" i2="" "">
                    <subfield label=""a"">bu,u,mu</subfield>
                    <subfield label=""b"">dc,di,dz</subfield>
                    <subfield label=""d"">R</subfield>
                </varfield>
                <varfield id=""020"" i1="" "" i2="" "">
                    <subfield label=""a"">978-82-02-29195-2</subfield>
                </varfield>
                <varfield id=""041"" i1="" "" i2="" "">
                    <subfield label=""h"">eng</subfield>
                </varfield>
                <varfield id=""090"" i1="" "" i2="" "">
                    <subfield label=""c"">n781.542</subfield>
                    <subfield label=""d"">ROW</subfield>
                </varfield>
                <varfield id=""100"" i1="" "" i2=""0"">
                    <subfield label=""a"">Rowling, J.K.</subfield>
                    <subfield label=""d"">1965-</subfield>
                    <subfield label=""j"">eng.</subfield>
                </varfield>
                <varfield id=""240"" i1=""1"" i2=""0"">
                    <subfield label=""a"">Harry Potter and the deathly hallows</subfield>
                </varfield>
                <varfield id=""245"" i1=""1"" i2=""0"">
                    <subfield label=""a"">Harry Potter og dødstalismanene</subfield>
                    <subfield label=""c"">J.K. Rowling ; [oversetter: Torstein Bugge Høverstad]</subfield>
                    <subfield label=""h"">lydopptak</subfield>
                    <subfield label=""n"">III</subfield>
                    <subfield label=""p"">Atter en konge</subfield>
                </varfield>
                <varfield id=""250"" i1=""1"" i2=""0"">
                    <subfield label=""a"">Collector's edition</subfield>
                </varfield>
                <varfield id=""260"" i1="" "" i2="" "">
                    <subfield label=""a"">[Oslo]</subfield>
                    <subfield label=""b"">Cappelen Damm</subfield>
                    <subfield label=""c"">2008</subfield>
                </varfield>
                <varfield id=""300"" i1="" "" i2="" "">
                    <subfield label=""a"">2 CDer (24 t, 35 min)</subfield>
                </varfield>
                <varfield id=""440"" i1="" "" i2=""0"">
                    <subfield label=""a"">Harry Potter</subfield>
                    <subfield label=""v"">7</subfield>
                </varfield>
                <varfield id=""440"" i1="" "" i2=""0"">
                    <subfield label=""a"">Lommelydbok</subfield>
                </varfield>
                <varfield id=""500"" i1="" "" i2="" "">
                    <subfield label=""a"">Mp3-format</subfield>
                </varfield>
                <varfield id=""511"" i1="" "" i2="" "">
                    <subfield label=""a"">Lest av Torstein Bugge Høverstad</subfield>
                </varfield>
                <varfield id=""520"" i1="" "" i2="" "">
                    <subfield label=""a"">I &quot;Harry Potter og halvblodsprinsen&quot; ble det hengende igjen tre spørsmål: Dør Harry? Er Slur god eller ond? Er Humlesnurr virkelig død? I &quot;Harry Potter og dødstalismanene&quot; gis det svar på alle tre spørsmålene, pluss mye mer. Allerede i første kapittel kommer et grusomt dødsfall, noe som demonstrerer hvordan Voldemort og hans dødsetere både har fått makt og går hardere til verks. De holder til i huset til Lucius Malfang, og legger en plan for å ta Harry når han skal flyttes fra Hekkveien til et hemmelig sted. Harry fyller 17 år, og da mister han den spesielle beskyttelsen han har hatt gjennom å bo hos sin familie. I tillegg blir Humlesnurrs historie nøstet opp. Endelig får vi vite hva som er Harrys egentlige skjebne og hvordan det går med han og vennene hans det siste året på Galtvort. Syvende og siste bok i serien om Harry Potter er dystrere enn de foregående bøkene.</subfield>
                </varfield>
                <varfield id=""546"" i1="" "" i2="" "">
                    <subfield label=""a"">Norsk tale</subfield>
                </varfield>
                <varfield id=""599"" i1="" "" i2="" "">
                    <subfield label=""a"">173 kr</subfield>
                </varfield>
                <varfield id=""600"" i1="" "" i2="" "">
                  <subfield label=""a"">Knut Hamsun</subfield>
                  <subfield label=""d"">1859-1952</subfield>
                  <subfield label=""j"">Norsk</subfield>
                  <subfield label=""t"">Sult</subfield>
                </varfield>
                <varfield id=""610"" i1="" "" i2="" "">
                  <subfield label=""a"">Capgemini</subfield>
                  <subfield label=""b"">B-tech</subfield>
                  <subfield label=""q"">Forklaring</subfield>
                  <subfield label=""t"">Visjon</subfield>
                </varfield>         
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Harry Potter</subfield>
                    <subfield label=""q"">fiktiv person</subfield>
                </varfield>
                <varfield id=""651"" i1="" "" i2="" "">
                    <subfield label=""a"">England</subfield>
                </varfield>
                <varfield id=""655"" i1="" "" i2="" "">
                    <subfield label=""a"">Fantasy</subfield>
                </varfield>
                <varfield id=""700"" i1="" "" i2="" "">
                  <subfield label=""a"">Harald V</subfield>
                  <subfield label=""d"">1937-</subfield>
                  <subfield label=""j"">Norsk</subfield>
                  <subfield label=""e"">Konge</subfield>
                </varfield>
                <varfield id=""700"" i1="" "" i2="" "">
                  <subfield label=""a"">Kjell Inge Røkke</subfield>
                  <subfield label=""d"">1950-</subfield>
                  <subfield label=""j"">Norsk</subfield>
                  <subfield label=""e"">Investor</subfield>
                </varfield>
                <varfield id=""710"" i1="" "" i2="" "">
                  <subfield label=""a"">Cappelen Damm</subfield>
                  <subfield label=""b"">Salg</subfield>
                  <subfield label=""e"">Forlag</subfield>
                  <subfield label=""q"">Forklaring</subfield>
                </varfield>
                <varfield id=""740"" i1=""0"" i2=""0"">
                    <subfield label=""a"">HP</subfield>
                </varfield>
                <varfield id=""780"" i1=""0"" i2=""0"">
                    <subfield label=""t"">Harry Potter og halvblodsprinsen</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">KATALOG</subfield>
                    <subfield label=""b"">40</subfield>
                    <subfield label=""c"">20110324</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">0849</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH-UPD</subfield>
                    <subfield label=""b"">40</subfield>
                    <subfield label=""c"">20110324</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">0849</subfield>
                </varfield>
            </oai_marc>
        </metadata>
    </record>
    <session-id>PSQGNNC65MPCDKMD5UNGS3IH7P3CAAUY221CRR1F8K4FXIB5KH</session-id>
</find-doc>";
        }

        private string getAudioBookWithOrgXml()
        {
            return @"<find-doc>
  <record>
    <doc_number>123456789</doc_number>
    <metadata>
      <oai_marc>
        <fixfield id=""FMT"">BK</fixfield>
        <fixfield id=""LDR"">^^^^^nam^^^^^^^^^1</fixfield>
        <fixfield id=""008"">110106s2010^^^^^^^^^^^a^^^^^^^^^^1^mul^^</fixfield>
        <varfield id=""019"" i1="" "" i2="" "">
          <subfield label=""b"">l</subfield>
          <subfield label=""d"">R</subfield>
        </varfield>
        <varfield id=""020"" i1="" "" i2="" "">
          <subfield label=""a"">978-82-02-33225-9</subfield>
          <subfield label=""b"">ib.</subfield>
        </varfield>
        <varfield id=""041"" i1="" "" i2="" "">
          <subfield label=""a"">nobsweeng</subfield>
        </varfield>
        <varfield id=""090"" i1="" "" i2="" "">
          <subfield label=""c"">598.0948</subfield>
          <subfield label=""d"">LOE</subfield>
        </varfield>
        <varfield id=""110"" i1="" "" i2=""0"">
          <subfield label=""a"">Røde Kors</subfield>
          <subfield label=""b"">Hjelpekorpset</subfield>
          <subfield label=""q"">Hjelper folk</subfield>
        </varfield>
        <varfield id=""245"" i1=""1"" i2=""0"">
          <subfield label=""a"">Naiv. Super</subfield>
          <subfield label=""b"">Supert</subfield>
          <subfield label=""c"">Erlend Loe</subfield>
          <subfield label=""n"">5</subfield>
        </varfield>
        <varfield id=""250"" i1="" "" i2="" "">
          <subfield label=""a"">5. oppl.</subfield>
        </varfield>
        <varfield id=""260"" i1="" "" i2="" "">
          <subfield label=""a"">[Oslo]</subfield>
          <subfield label=""b"">Cappelen Damm</subfield>
          <subfield label=""c"">2010</subfield>
        </varfield>
        <varfield id=""300"" i1="" "" i2="" "">
          <subfield label=""a"">205 s.</subfield>
        </varfield>
        <varfield id=""440"" i1="" "" i2=""0"">
          <subfield label=""a"">Favoritt</subfield>
          <subfield label=""v"">5</subfield>
        </varfield>
        <varfield id=""503"" i1="" "" i2="" "">
          <subfield label=""a"">1. utg.: Oslo : Cappelen, 1996</subfield>
        </varfield>
        <varfield id=""505"" i1="" "" i2="" "">
          <subfield label=""a"">Innhold: Et liv i fellesskap ; Bibelens bønnebok</subfield>
        </varfield>
        <varfield id=""599"" i1="" "" i2="" "">
          <subfield label=""a"">200 kr</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Kristendom</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Buddhisme</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Religionsvitenskap</subfield>
        </varfield>
        <varfield id=""651"" i1="" "" i2="" "">
          <subfield label=""a"">Norge</subfield>
        </varfield>
        <varfield id=""651"" i1="" "" i2="" "">
          <subfield label=""a"">Rogaland</subfield>
        </varfield>
        <varfield id=""655"" i1="" "" i2="" "">
          <subfield label=""a"">Action</subfield>
        </varfield>
        <varfield id=""655"" i1="" "" i2="" "">
          <subfield label=""a"">Komedie</subfield>
        </varfield>
        <varfield id=""850"" i1="" "" i2="" "">
          <subfield label=""a"">stavangb</subfield>
          <subfield label=""c"">LOE</subfield>
          <subfield label=""d"">2010</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">LHA</subfield>
          <subfield label=""b"">30</subfield>
          <subfield label=""c"">20110106</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1254</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">BATCH-UPD</subfield>
          <subfield label=""b"">30</subfield>
          <subfield label=""c"">20110106</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1254</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">KATALOG</subfield>
          <subfield label=""b"">40</subfield>
          <subfield label=""c"">20110201</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1136</subfield>
        </varfield>
      </oai_marc>
    </metadata>
  </record>
  <session-id>XXIT5PJTANKBX77H4PR6X8VJMN3BTGXFEURFSCSIH4FBMJSXHX</session-id>
</find-doc>";
        }

        private string getAudioBookWithStdTitleXml()
        {
            return @"<find-doc>
  <record>
    <metadata>
      <doc_number>123456789</doc_number>
      <oai_marc>
        <fixfield id=""FMT"">BK</fixfield>
        <fixfield id=""LDR"">^^^^^nam^^^^^^^^^1</fixfield>
        <fixfield id=""008"">110106s2010^^^^^^^^^^^a^^^^^^^^^^1^mul^^</fixfield>
        <varfield id=""019"" i1="" "" i2="" "">
          <subfield label=""b"">l</subfield>
          <subfield label=""d"">R</subfield>
        </varfield>
        <varfield id=""020"" i1="" "" i2="" "">
          <subfield label=""a"">978-82-02-33225-9</subfield>
          <subfield label=""b"">ib.</subfield>
        </varfield>
        <varfield id=""041"" i1="" "" i2="" "">
          <subfield label=""a"">nobsweeng</subfield>
        </varfield>
        <varfield id=""090"" i1="" "" i2="" "">
          <subfield label=""c"">598.0948</subfield>
          <subfield label=""d"">LOE</subfield>
        </varfield>
        <varfield id=""130"" i1="" "" i2=""0"">
          <subfield label=""a"">Røde Kors</subfield>
        </varfield>
        <varfield id=""245"" i1=""1"" i2=""0"">
          <subfield label=""a"">Naiv. Super</subfield>
          <subfield label=""b"">Supert</subfield>
          <subfield label=""c"">Erlend Loe</subfield>
          <subfield label=""n"">5</subfield>
        </varfield>
        <varfield id=""250"" i1="" "" i2="" "">
          <subfield label=""a"">5. oppl.</subfield>
        </varfield>
        <varfield id=""260"" i1="" "" i2="" "">
          <subfield label=""a"">[Oslo]</subfield>
          <subfield label=""b"">Cappelen Damm</subfield>
          <subfield label=""c"">2010</subfield>
        </varfield>
        <varfield id=""300"" i1="" "" i2="" "">
          <subfield label=""a"">205 s.</subfield>
        </varfield>
        <varfield id=""440"" i1="" "" i2=""0"">
          <subfield label=""a"">Favoritt</subfield>
          <subfield label=""v"">5</subfield>
        </varfield>
        <varfield id=""503"" i1="" "" i2="" "">
          <subfield label=""a"">1. utg.: Oslo : Cappelen, 1996</subfield>
        </varfield>
        <varfield id=""505"" i1="" "" i2="" "">
          <subfield label=""a"">Innhold: Et liv i fellesskap ; Bibelens bønnebok</subfield>
        </varfield>
        <varfield id=""599"" i1="" "" i2="" "">
          <subfield label=""a"">200 kr</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Kristendom</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Buddhisme</subfield>
        </varfield>
        <varfield id=""650"" i1="" "" i2="" "">
          <subfield label=""a"">Religionsvitenskap</subfield>
        </varfield>
        <varfield id=""651"" i1="" "" i2="" "">
          <subfield label=""a"">Norge</subfield>
        </varfield>
        <varfield id=""651"" i1="" "" i2="" "">
          <subfield label=""a"">Rogaland</subfield>
        </varfield>
        <varfield id=""655"" i1="" "" i2="" "">
          <subfield label=""a"">Action</subfield>
        </varfield>
        <varfield id=""655"" i1="" "" i2="" "">
          <subfield label=""a"">Komedie</subfield>
        </varfield>
        <varfield id=""850"" i1="" "" i2="" "">
          <subfield label=""a"">stavangb</subfield>
          <subfield label=""c"">LOE</subfield>
          <subfield label=""d"">2010</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">LHA</subfield>
          <subfield label=""b"">30</subfield>
          <subfield label=""c"">20110106</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1254</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">BATCH-UPD</subfield>
          <subfield label=""b"">30</subfield>
          <subfield label=""c"">20110106</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1254</subfield>
        </varfield>
        <varfield id=""CAT"" i1="" "" i2="" "">
          <subfield label=""a"">KATALOG</subfield>
          <subfield label=""b"">40</subfield>
          <subfield label=""c"">20110201</subfield>
          <subfield label=""l"">NOR01</subfield>
          <subfield label=""h"">1136</subfield>
        </varfield>
      </oai_marc>
    </metadata>
  </record>
  <session-id>XXIT5PJTANKBX77H4PR6X8VJMN3BTGXFEURFSCSIH4FBMJSXHX</session-id>
</find-doc>";
        }

        private string getJournalXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<find-doc>
    <record>
        <metadata>
            <oai_marc>
                <fixfield id=""FMT"">SE</fixfield>
                <fixfield id=""LDR"">00622cas^^22002411^^45^^</fixfield>
                <fixfield id=""BAS"">25</fixfield>
                <fixfield id=""008"">880129^^^^^^^^^^^^^^^pa^^^^^^^^^^0^eng^^</fixfield>
                <varfield id=""015"" i1="" "" i2="" "">
                    <subfield label=""a"">0175989</subfield>
                    <subfield label=""b"">media-f</subfield>
                </varfield>
                <varfield id=""019"" i1="" "" i2="" "">
                    <subfield label=""b"">j</subfield>
                </varfield>
                <varfield id=""022"" i1="" "" i2="" "">
                    <subfield label=""a"">0163-7053</subfield>
                </varfield>
                <varfield id=""082"" i1="" "" i2="" "">
                    <subfield label=""a"">051</subfield>
                    <subfield label=""z"">h</subfield>
                </varfield>
                <varfield id=""090"" i1="" "" i2="" "">
                    <subfield label=""b"">q</subfield>
                    <subfield label=""d"">NEW</subfield>
                </varfield>
                <varfield id=""091"" i1="" "" i2="" "">
                    <subfield label=""b"">p</subfield>
                    <subfield label=""c"">a</subfield>
                    <subfield label=""f"">0</subfield>
                    <subfield label=""h"">eng</subfield>
                </varfield>
                <varfield id=""245"" i1=""0"" i2="" "">
                    <subfield label=""a"">Newsweek</subfield>
                    <subfield label=""b"">the international newsmagazine</subfield>
                </varfield>
                <varfield id=""260"" i1="" "" i2="" "">
                    <subfield label=""a"">New York</subfield>
                    <subfield label=""b"">Newsweek</subfield>
                    <subfield label=""c"">[19-]-</subfield>
                </varfield>
                <varfield id=""300"" i1="" "" i2="" "">
                    <subfield label=""b"">Ill.</subfield>
                </varfield>
                <varfield id=""310"" i1="" "" i2="" "">
                    <subfield label=""a"">51 nummer pr. år</subfield>
                </varfield>
                <varfield id=""350"" i1="" "" i2="" "">
                    <subfield label=""a"">671 kr</subfield>
                </varfield>
                <varfield id=""599"" i1="" "" i2="" "">
                    <subfield label=""a"">50 kr</subfield>
                </varfield>
                <varfield id=""590"" i1="" "" i2="" "">
                    <subfield label=""a"">Biblioteket har: Siste årg.</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Nyheter</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Tidsskrifter</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20041103</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1349</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b""></subfield>
                    <subfield label=""c"">19971213</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1829</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b""></subfield>
                    <subfield label=""c"">19990531</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1604</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b""></subfield>
                    <subfield label=""c"">19991116</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1000</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">STVGBIBL</subfield>
                    <subfield label=""b"">10</subfield>
                    <subfield label=""c"">20000209</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1520</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">STVGBIBL</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20011102</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1118</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20041104</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1305</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20050531</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1404</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">FAKTA</subfield>
                    <subfield label=""b"">20</subfield>
                    <subfield label=""c"">20051227</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1217</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">FAKTA</subfield>
                    <subfield label=""b"">20</subfield>
                    <subfield label=""c"">20051227</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1218</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20060818</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1200</subfield>
                </varfield>
            </oai_marc>
        </metadata>
    </record>
    <session-id>EX5XBBIM7KKG2PTCXX4ALEYGF6125NDYG8AE7EXKAMUUPUS8E8</session-id>
</find-doc>";
        }

        private string getCdPopularMusicGroupXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<find-doc>
    <record>
        <metadata>
            <oai_marc>
                <fixfield id=""FMT"">MU</fixfield>
                <fixfield id=""LDR"">^^^^^njm^^^^^^^^^1</fixfield>
                <fixfield id=""007"">sc</fixfield>
                <fixfield id=""008"">081004s2006^^^^^^^^^^^a^^^^^^^^^^^^nno^^</fixfield>
                <varfield id=""019"" i1="" "" i2="" "">
                    <subfield label=""b"">dc,dg</subfield>
                </varfield>
                <varfield id=""028"" i1=""0"" i2="" "">
                    <subfield label=""a"">176 535-9</subfield>
                    <subfield label=""b"">Universal</subfield>
                </varfield>
                <varfield id=""082"" i1="" "" i2="" "">
                    <subfield label=""a"">781.64</subfield>
                    <subfield label=""z"">h</subfield>
                </varfield>
                <varfield id=""090"" i1="" "" i2="" "">
                    <subfield label=""d"">MOD</subfield>
                </varfield>
                <varfield id=""110"" i1="" "" i2=""0"">
                    <subfield label=""a"">Mods</subfield>
                </varfield>
                <varfield id=""245"" i1=""1"" i2=""0"">
                    <subfield label=""a"">Gje meg litt merr</subfield>
                    <subfield label=""h"">CD</subfield>
                    <subfield label=""b"">de beste</subfield>
                    <subfield label=""c"">Mods</subfield>
                </varfield>
                <varfield id=""260"" i1="" "" i2="" "">
                    <subfield label=""a"">[Norge]</subfield>
                    <subfield label=""b"">Universal</subfield>
                    <subfield label=""c"">2006</subfield>
                </varfield>
                <varfield id=""300"" i1="" "" i2="" "">
                    <subfield label=""a"">1 kompaktplate</subfield>
                    <subfield label=""b"">digital, stereo</subfield>
                    <subfield label=""c"">12 cm</subfield>
                </varfield>
                <varfield id=""505"" i1="" "" i2="" "">
                    <subfield label=""a"">Innhold: Gje meg litt merr ; Belinda ; Revansj ; Me to går alltid aleina ; Amerika ; Bare i nått ; Eg e så forelska ; Ett år e gått ; Tore Tang ; Fint at du vil ; Eg vil hjem ; Hjelp meg ; Militæret ; Alexander ; Eg kom ikkje inn ; Regn ; Meg må du hilsa på ; Ikkje plag meg ; Bli med oss ; Livets roulette ; Another day ; Bahama Mama</subfield>
                </varfield>
                <varfield id=""511"" i1="" "" i2="" "">
                    <subfield label=""a"">Utøvere: Kurt Ø. Olsen, Helge Hummervoll, Leif Nilsen, Morten A. Knutsen, Torkild Viig, Runar Bjaalid, Tor Øyvind Syvertsen</subfield>
                </varfield>
                <varfield id=""599"" i1="" "" i2="" "">
                    <subfield label=""a"">179 kr</subfield>
                </varfield>
                <varfield id=""651"" i1="" "" i2="" "">
                    <subfield label=""a"">Norge</subfield>
                </varfield>
                <varfield id=""651"" i1="" "" i2="" "">
                    <subfield label=""a"">Rogaland</subfield>
                </varfield>
                <varfield id=""651"" i1="" "" i2="" "">
                    <subfield label=""a"">Stavanger</subfield>
                </varfield>
                <varfield id=""652"" i1="" "" i2="" "">
                    <subfield label=""a"">Popmusikk</subfield>
                </varfield>
                <varfield id=""652"" i1="" "" i2="" "">
                    <subfield label=""a"">Rock</subfield>
                </varfield>
                <varfield id=""690"" i1="" "" i2="" "">
                    <subfield label=""a"">Musikksamling Stavanger</subfield>
                </varfield>
                <varfield id=""700"" i1="" "" i2=""0"">
                    <subfield label=""a"">Olsen, Kurt Ø.</subfield>
                    <subfield label=""d"">1961-</subfield>
                    <subfield label=""j"">n.</subfield>
                    <subfield label=""e"">utøv.</subfield>
                </varfield>
                <varfield id=""700"" i1="" "" i2=""0"">
                    <subfield label=""a"">Hummervoll, Helge</subfield>
                    <subfield label=""d"">1961-</subfield>
                    <subfield label=""j"">n.</subfield>
                    <subfield label=""e"">utøv.</subfield>
                </varfield>
                <varfield id=""700"" i1="" "" i2=""0"">
                    <subfield label=""a"">Nilsen, Leif</subfield>
                    <subfield label=""j"">n.</subfield>
                    <subfield label=""q"">artist fra Stavanger</subfield>
                    <subfield label=""e"">utøv.</subfield>
                </varfield>
                <varfield id=""700"" i1="" "" i2=""0"">
                    <subfield label=""a"">Abel, Morten</subfield>
                    <subfield label=""d"">1964-</subfield>
                    <subfield label=""j"">n.</subfield>
                    <subfield label=""e"">utøv.</subfield>
                </varfield>
                <varfield id=""700"" i1="" "" i2=""0"">
                    <subfield label=""a"">Viig, Torkild</subfield>
                    <subfield label=""d"">1963-</subfield>
                    <subfield label=""j"">n.</subfield>
                    <subfield label=""e"">utøv.</subfield>
                </varfield>
                <varfield id=""700"" i1="" "" i2=""0"">
                    <subfield label=""a"">Bjaalid, Rune</subfield>
                    <subfield label=""d"">1961-</subfield>
                    <subfield label=""j"">n.</subfield>
                    <subfield label=""e"">utøv.</subfield>
                </varfield>
                <varfield id=""700"" i1="" "" i2=""0"">
                    <subfield label=""a"">Syvertsen, Tor Øyvind</subfield>
                    <subfield label=""d"">1962-</subfield>
                    <subfield label=""j"">n.</subfield>
                    <subfield label=""e"">utøv.</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">KATALOG</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20081004</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1125</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH-UPD</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20090821</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1621</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH-UPD</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20101025</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1145</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH-UPD</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20101025</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1147</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH-UPD</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20101025</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1217</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH-UPD</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20101025</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1220</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">KATALOG</subfield>
                    <subfield label=""b"">40</subfield>
                    <subfield label=""c"">20101202</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1425</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH-UPD</subfield>
                    <subfield label=""b"">40</subfield>
                    <subfield label=""c"">20110329</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1229</subfield>
                </varfield>
            </oai_marc>
        </metadata>
    </record>
    <session-id>VL2J7DKSS61YBY1VQLEK539UCUVLRQ91TV33QX5CSC6RSXD4BD</session-id>
</find-doc>";
        }

        private string getCdPopluarArtistXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<find-doc>
    <record>
        <metadata>
            <oai_marc>
                <fixfield id=""FMT"">MU</fixfield>
                <fixfield id=""LDR"">^^^^^njm^^^^^^^^^1</fixfield>
                <fixfield id=""007"">sc</fixfield>
                <fixfield id=""008"">070327s2006^^^^^^^^^^^a^^^^^^^^^^^^eng^^</fixfield>
                <varfield id=""019"" i1="" "" i2="" "">
                    <subfield label=""b"">dc,dg</subfield>
                </varfield>
                <varfield id=""028"" i1=""0"" i2="" "">
                    <subfield label=""a"">0094638067603</subfield>
                    <subfield label=""b"">EMI</subfield>
                </varfield>
                <varfield id=""082"" i1="" "" i2="" "">
                    <subfield label=""a"">781.64</subfield>
                    <subfield label=""z"">h</subfield>
                </varfield>
                <varfield id=""090"" i1="" "" i2="" "">
                    <subfield label=""d"">ABE</subfield>
                </varfield>
                <varfield id=""100"" i1="" "" i2=""0"">
                    <subfield label=""a"">Abel, Morten</subfield>
                    <subfield label=""d"">1964-</subfield>
                    <subfield label=""j"">n.</subfield>
                </varfield>
                <varfield id=""245"" i1=""1"" i2=""0"">
                    <subfield label=""a"">Some of us will make it</subfield>
                    <subfield label=""h"">CD</subfield>
                    <subfield label=""c"">Morten Abel</subfield>
                </varfield>
                <varfield id=""260"" i1="" "" i2="" "">
                    <subfield label=""a"">London</subfield>
                    <subfield label=""b"">Virgin</subfield>
                    <subfield label=""c"">2006</subfield>
                </varfield>
                <varfield id=""300"" i1="" "" i2="" "">
                    <subfield label=""a"">1 kompaktplate</subfield>
                    <subfield label=""b"">digital, stereo</subfield>
                    <subfield label=""c"">12 cm</subfield>
                </varfield>
                <varfield id=""505"" i1="" "" i2="" "">
                    <subfield label=""a"">Innhold: Swim like a fish ; Big brother ; Rather be lonely ; River Phoenix ; Stars ; Such a pain ; Some of us will make it ; You can come ; Walk back home ; Down and out ; Waiting at the gate</subfield>
                </varfield>
                <varfield id=""599"" i1="" "" i2="" "">
                    <subfield label=""a"">179 kr</subfield>
                </varfield>
                <varfield id=""651"" i1="" "" i2="" "">
                    <subfield label=""a"">Norge</subfield>
                </varfield>
                <varfield id=""651"" i1="" "" i2="" "">
                    <subfield label=""a"">Stavanger</subfield>
                </varfield>
                <varfield id=""651"" i1="" "" i2="" "">
                    <subfield label=""a"">Rogaland</subfield>
                </varfield>
                <varfield id=""652"" i1="" "" i2="" "">
                    <subfield label=""a"">Popmusikk</subfield>
                </varfield>
                <varfield id=""690"" i1="" "" i2="" "">
                    <subfield label=""a"">Musikksamling Stavanger</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">KATALOG</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20070327</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">0754</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH-UPD</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20090821</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1621</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH-UPD</subfield>
                    <subfield label=""b"">30</subfield>
                    <subfield label=""c"">20120215</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1347</subfield>
                </varfield>
            </oai_marc>
        </metadata>
    </record>
    <session-id>Q6KEQY8GHVX1173QXERYX5PPSINEAA7NGEDCM3RB2QFCG17UDT</session-id>
</find-doc>";
        }

        private string getSheetMusicXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<find-doc>
    <record>
        <metadata>
            <oai_marc>
                <fixfield id=""FMT"">MU</fixfield>
                <fixfield id=""LDR"">00579ccm^^22002171^^45^^</fixfield>
                <fixfield id=""BAS"">25</fixfield>
                <fixfield id=""BAS"">30</fixfield>
                <fixfield id=""008"">823112^^^^^^^^^^^^^^^^a^^^^^^^^^^0^^^^^^</fixfield>
                <varfield id=""015"" i1="" "" i2="" "">
                    <subfield label=""a"">0117418</subfield>
                    <subfield label=""b"">media-f</subfield>
                </varfield>
                <varfield id=""019"" i1="" "" i2="" "">
                    <subfield label=""b"">c</subfield>
                </varfield>
                <varfield id=""082"" i1="" "" i2="" "">
                    <subfield label=""a"">785.5714</subfield>
                    <subfield label=""z"">h</subfield>
                </varfield>
                <varfield id=""090"" i1="" "" i2="" "">
                    <subfield label=""c"">785.5714</subfield>
                    <subfield label=""d"">BAC</subfield>
                </varfield>
                <varfield id=""091"" i1="" "" i2="" "">
                    <subfield label=""f"">0</subfield>
                </varfield>
                <varfield id=""100"" i1="" "" i2="" "">
                    <subfield label=""a"">Bach, Carl Philipp Emanuel</subfield>
                    <subfield label=""d"">1714-1788</subfield>
                    <subfield label=""j"">t.</subfield>
                </varfield>
                <varfield id=""245"" i1="" "" i2="" "">
                    <subfield label=""a"">March (fanfare) for 3 trumpets and timpani</subfield>
                </varfield>
                <varfield id=""260"" i1="" "" i2="" "">
                    <subfield label=""a"">N.Y.</subfield>
                    <subfield label=""b"">Marks Music</subfield>
                    <subfield label=""c"">u.å.</subfield>
                </varfield>
                <varfield id=""300"" i1="" "" i2="" "">
                    <subfield label=""a"">b. 4 st.</subfield>
                </varfield>
                <varfield id=""599"" i1="" "" i2="" "">
                    <subfield label=""a"">0035</subfield>
                </varfield>
                <varfield id=""500"" i1="" "" i2="" "">
                    <subfield label=""a"">MS 1310</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Kammermusikk</subfield>
                </varfield>
                <varfield id=""650"" i1="" "" i2="" "">
                    <subfield label=""a"">Marsj</subfield>
                </varfield>
                <varfield id=""658"" i1="" "" i2="" "">
                    <subfield label=""a"">Trompet 3</subfield>
                    <subfield label=""a"">Pauker</subfield>
                    <subfield label=""b"">04</subfield>
                </varfield>
                <varfield id=""850"" i1="" "" i2="" "">
                    <subfield label=""a"">stavangb</subfield>
                    <subfield label=""c"">785.5714 BAC</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20041103</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1514</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b""></subfield>
                    <subfield label=""c"">19971213</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1740</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a""></subfield>
                    <subfield label=""b""></subfield>
                    <subfield label=""c"">19990531</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1428</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20010816</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1307</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20041104</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1244</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""a"">BATCH</subfield>
                    <subfield label=""b"">00</subfield>
                    <subfield label=""c"">20050531</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1341</subfield>
                </varfield>
                <varfield id=""CAT"" i1="" "" i2="" "">
                    <subfield label=""c"">20060818</subfield>
                    <subfield label=""l"">NOR01</subfield>
                    <subfield label=""h"">1145</subfield>
                </varfield>
            </oai_marc>
        </metadata>
    </record>
    <session-id>2AR541IJ3QERCVPA44B36DF71HYVT1MJAA78QF32DAP96T1651</session-id>
</find-doc>";
        }

        private string getDocumentCircItemsXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<circ-status>
    <item-data>
        <z30-description></z30-description>
        <loan-status>4 uker</loan-status>
        <due-date>08/08/12</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Madla</sub-library>
        <collection>VOKS</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031186112</barcode>
        <opac-note></opac-note>
    </item-data>
    <item-data>
        <z30-description></z30-description>
        <loan-status>4 uker</loan-status>
        <due-date>On Shelf</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Hovedbibl.</sub-library>
        <collection>Kulturavdelingen</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031180135</barcode>
        <opac-note></opac-note>
    </item-data>
    <item-data>
        <z30-description></z30-description>
        <loan-status>4 uker</loan-status>
        <due-date>On Shelf</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Hovedbibl.</sub-library>
        <collection>Kulturavdelingen</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031180136</barcode>
        <opac-note></opac-note>
    </item-data>
    <item-data>
        <z30-description></z30-description>
        <loan-status>4 uker</loan-status>
        <due-date>13/08/12</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Hovedbibl.</sub-library>
        <collection>Kulturavdelingen</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031180134</barcode>
        <opac-note></opac-note>
    </item-data>
    <item-data>
        <z30-description></z30-description>
        <loan-status>4 uker</loan-status>
        <due-date>04/08/12</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Hovedbibl.</sub-library>
        <collection>Kulturavdelingen</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031180131</barcode>
        <opac-note></opac-note>
    </item-data>
    <item-data>
        <z30-description></z30-description>
        <loan-status>Stavanger fengsel</loan-status>
        <due-date>Stavanger fengsel</due-date>
        <due-hour>24:00</due-hour>
        <sub-library>Stavanger fengsel</sub-library>
        <collection>FENGS</collection>
        <location>Skjønnlitteratur</location>
        <pages></pages>
        <no-requests></no-requests>
        <location-2></location-2>
        <barcode>11031180133</barcode>
        <opac-note></opac-note>
    </item-data>
    <session-id>IDEQKF24FAEP5FQRAC4P2TCYCIRAMY4K16E185AUE2UYPKA81K</session-id>
</circ-status>
 ";
        }

        private string getDocumentItemsXml()
        {
            return @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
<item-data>
    <item>
        <rec-key>000611167000110</rec-key>
        <barcode>11031180135</barcode>
        <sub-library>Hovedbibl.</sub-library>
        <collection>KULT</collection>
        <item-status>04</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
    </item>
    <item>
        <rec-key>000611167000160</rec-key>
        <barcode>11031180136</barcode>
        <sub-library>Hovedbibl.</sub-library>
        <collection>KULT</collection>
        <item-status>04</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
    </item>
    <item>
        <rec-key>000611167000170</rec-key>
        <barcode>11031180133</barcode>
        <sub-library>Stavanger fengsel</sub-library>
        <collection>FENGS</collection>
        <item-status>80</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
    </item>
    <item>
        <rec-key>000611167000180</rec-key>
        <barcode>11031180134</barcode>
        <sub-library>Hovedbibl.</sub-library>
        <collection>KULT</collection>
        <item-status>04</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
        <loan-status>A</loan-status>
        <loan-in-transit>N</loan-in-transit>
        <loan-due-date>20120813</loan-due-date>
        <loan-due-hour>2400</loan-due-hour>
    </item>
    <item>
        <rec-key>000611167000190</rec-key>
        <barcode>11031180131</barcode>
        <sub-library>Hovedbibl.</sub-library>
        <collection>KULT</collection>
        <item-status>04</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
        <loan-status>A</loan-status>
        <loan-in-transit>N</loan-in-transit>
        <loan-due-date>20120804</loan-due-date>
        <loan-due-hour>2400</loan-due-hour>
    </item>
    <item>
        <rec-key>000611167000200</rec-key>
        <barcode>11031186112</barcode>
        <sub-library>Madla</sub-library>
        <collection>VOKS</collection>
        <item-status>04</item-status>
        <note></note>
        <call-no-1>Skjønnlitteratur</call-no-1>
        <call-no-2></call-no-2>
        <description></description>
        <chronological-i></chronological-i>
        <chronological-j></chronological-j>
        <chronological-k></chronological-k>
        <enumeration-a></enumeration-a>
        <enumeration-b></enumeration-b>
        <enumeration-c></enumeration-c>
        <library>NOR50</library>
        <on-hold>N</on-hold>
        <requested>N</requested>
        <expected>N</expected>
        <loan-status>A</loan-status>
        <loan-in-transit>N</loan-in-transit>
        <loan-due-date>20120808</loan-due-date>
        <loan-due-hour>2400</loan-due-hour>
    </item>
    <session-id>CHJMYTFX3E2NUK4KMTSQ2J5KTEPSFJLGS4NMM1QF836MB7KRCD</session-id>
</item-data>";
        }

    }
}
