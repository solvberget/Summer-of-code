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
            
        }

        [Test]
        public void GetBookFromXmlTest()
        {
            var book = Book.GetBookFromFindDocXml(getBookXml());

            Assert.AreEqual("978-82-02-33225-9", book.Isbn);
            Assert.AreEqual("Loe, Erlend", book.Author);
            Assert.AreEqual("5", book.Numbering);
            Assert.AreEqual("5. oppl.", book.Edition);
            
        }

        [Test]
        public void GetFilmFromXmlTest()
        {
            var film = Film.GetFilmFromFindDocXml(getFilmXml());

            Assert.AreEqual("Max Manus", film.Title);
        }

        [Test]
        public void GetDocumentTypeFromXml()
        {
            var film = Film.GetFilmFromFindDocXml(getFilmXml());

            Assert.AreEqual("ee", film.DocumentType);
            
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
        </varfield>
        <varfield id=""503"" i1="" "" i2="" "">
          <subfield label=""a"">1. utg.: Oslo : Cappelen, 1996</subfield>
        </varfield>
        <varfield id=""599"" i1="" "" i2="" "">
          <subfield label=""a"">200 kr</subfield>
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
                <varfield id=""245"" i1=""0"" i2=""0"">
                    <subfield label=""a"">Max Manus</subfield>
                    <subfield label=""h"">DVD</subfield>
                    <subfield label=""c"">regi: Espen Sandberg og Joachim Rønning ; manuskript Thomas Nordset-Tiller</subfield>
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
                <varfield id=""651"" i1="" "" i2="" "">
                    <subfield label=""a"">Norge</subfield>
                    <subfield label=""q"">produksjonsland</subfield>
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
    }
}
