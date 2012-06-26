using System.Linq;
using NUnit.Framework;
using Solvberget.Domain.DTO;

namespace Solvberget.Service.Tests.DTOTests
{
    [TestFixture]
    public class TestModelConvertionFromXml
    {
        [Test]
        public void GetDocumentFromXmlTest()
        {
            var media = Document.GetDocumentFromFindDocXml(getBookXml());

            Assert.AreEqual('a', media.TargetGroup);

            Assert.IsTrue(media.IsFiction);

            Assert.AreEqual(3, media.Language.Length);
            Assert.AreEqual("nob", media.Language);

            Assert.AreEqual("l", media.DocumentType);

            Assert.AreEqual("LOE", media.LocationCode);
            
            Assert.AreEqual("Naiv. Super", media.Title);

            Assert.AreEqual("Supert", media.SubTitle);

            foreach(string person in media.InvolvedPersons)
                Assert.AreEqual("Erlend Loe", person);

            Assert.AreEqual("[Oslo]", media.PlacePublished);

            Assert.AreEqual("Cappelen Damm", media.Publisher);

            Assert.AreEqual(2010, media.PublishedYear);

            Assert.AreEqual("Favoritt", media.SeriesTitle);

            Assert.AreEqual("5", media.SeriesNumber);

        }

        [Test]
        public void GetBookFromXmlTest()
        {
            var book = Book.GetBookFromFindDocXml(getBookXml());

            Assert.AreEqual("978-82-02-33225-9", book.Isbn);

            Assert.AreEqual("598.0948", book.ClassificationNr);

            Assert.AreEqual("Loe, Erlend", book.Author);

            Assert.AreEqual("5", book.Numbering);

            Assert.AreEqual("5. oppl.", book.Edition);

            Assert.AreEqual("205 s.", book.NumberOfPages);

            Assert.AreEqual("Innhold: Et liv i fellesskap ; Bibelens bønnebok", book.Content);

            var i = 0;
            foreach (string subject in book.Subject)
            {
                if (i==0)
                    Assert.AreEqual("Kristendom", subject);
                if (i==1)
                    Assert.AreEqual("Buddhisme", subject);
                if (i==2)
                    Assert.AreEqual("Religionsvitenskap", subject);
                i++;
            }

            var j = 0;
            foreach (string place in book.ReferencedPlaces)
            {
                if (j == 0)
                    Assert.AreEqual("Norge", place);
                if (j == 1)
                    Assert.AreEqual("Rogaland", place);
                j++;
            }

            var k = 0;
            foreach (string genre in book.Genre)
            {
                if (k == 0)
                    Assert.AreEqual("Action", genre);
                if (k == 1)
                    Assert.AreEqual("Komedie", genre);
                k++;
            }

        }

        [Test]
        public void GetFilmFromXmlTest()
        {
            var film = Film.GetFilmFromFindDocXml(getFilmXml());

            Assert.AreEqual("7041271735935", film.Ean);

            Assert.AreEqual("nob", film.SpokenLanguage.FirstOrDefault());
            Assert.AreEqual(1, film.SpokenLanguage.Count());

            Assert.AreEqual("nob", film.SubtitleLanguage.FirstOrDefault());
            Assert.AreEqual(4, film.SubtitleLanguage.Count());

            Assert.AreEqual("Max Manus", film.OriginalTitle);

            Assert.AreEqual("1", film.Numbering);

            Assert.AreEqual("Max", film.PartTitle);

            Assert.AreEqual("Collector's edition", film.Edition);

            Assert.AreEqual("2008", film.ProductionYear);

            Assert.AreEqual("2 videoplater (DVD-video)(1 t 53 min)", film.TypeAndNumberOfDiscs);

            Assert.AreEqual("Innhold: Lily's theme ; Statues ; Neville the hero ; Courtyard apocalypse ; Severus and Lily ; Harry's sacrifice ; The resurrection stone ; A new beginning ; Lily's lullaby", film.Contents);

            Assert.AreEqual("Rolleliste: Aksel Hennie, Nicolai Cleve Broch, Christian Rubeck, Knut Joner, Mats Eldøen, Pål Sverre Valheim, Agnes Kittelsen, Viktoria Winge, Kyrre Haugen Sydness, Jakob Oftebro, Petter Næss", film.Actors);

            Assert.AreEqual("Aldersgrense: 15 år", film.AgeLimit);

            Assert.AreEqual("Max Manus", film.NorwegianTitle);

            Assert.AreEqual("Undervisning", film.Subject);

            Assert.AreEqual("Norge", film.GeoraphicSubject);

            Assert.AreEqual("Popmusikk", film.CompositionType);

            Assert.AreEqual("Drama", film.Genre);

        }

        [Test]
        public void GetDocumentTypeFromXml()
        {
            var film = Film.GetFilmFromFindDocXml(getFilmXml());

            Assert.AreEqual("ee", film.DocumentType);
            
        }

        [Test]
        public void GetAudioBookFromXmlTest()
        {
            var audioBook = AudioBook.GetAudioBookFromFindDocXml(getAudioBookXML());

            Assert.AreEqual("978-82-02-29195-2", audioBook.Isbn);

            Assert.AreEqual("n781.542", audioBook.ClassificationNumber);

            Assert.AreEqual("Rowling, J.K.", audioBook.Author);

            Assert.AreEqual("1965-", audioBook.AuthorLivingYears);

            Assert.AreEqual("eng.", audioBook.AuthorNationality);

            Assert.AreEqual("III", audioBook.Numbering);

            Assert.AreEqual("Atter en konge", audioBook.PartTitle);

            Assert.AreEqual("Collector's edition", audioBook.Edition);

            Assert.AreEqual("2 CDer (24 t, 35 min)", audioBook.TypeAndNumberOfDiscs);

            Assert.AreEqual("Harry Potter", audioBook.Subject);

            Assert.AreEqual("England", audioBook.ReferencedPlaces.ElementAt(0));

            Assert.AreEqual("Fantasy", audioBook.Genre.ElementAt(0));

        }

        private string getBookXml()
        {
            return @"<find-doc>
  <record>
    <metadata>
      <oai_marc>
        <fixfield id=""FMT"">BK</fixfield>
        <fixfield id=""LDR"">^^^^^nam^^^^^^^^^1</fixfield>
        <fixfield id=""008"">110106s2010^^^^^^^^^^^a^^^^^^^^^^1^nob^^</fixfield>
        <varfield id=""019"" i1="" "" i2="" "">
          <subfield label=""b"">l</subfield>
          <subfield label=""d"">R</subfield>
        </varfield>
        <varfield id=""020"" i1="" "" i2="" "">
          <subfield label=""a"">978-82-02-33225-9</subfield>
          <subfield label=""b"">ib.</subfield>
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
        <metadata>
            <oai_marc>
                <fixfield id=""FMT"">VM</fixfield>
                <fixfield id=""LDR"">^^^^^ngm^^^^^^^^^1</fixfield>
                <fixfield id=""007"">vd</fixfield>
                <fixfield id=""008"">090626s2009^^^^^^^^^^^a^^^^^^^^^^1^nob^^</fixfield>
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
                <varfield id=""700"" i1="" "" i2=""0"">
                    <subfield label=""a"">Høverstad, Torstein Bugge</subfield>
                    <subfield label=""d"">1944-</subfield>
                    <subfield label=""j"">n.</subfield>
                    <subfield label=""e"">innl. og overs.</subfield>
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
    }
}
