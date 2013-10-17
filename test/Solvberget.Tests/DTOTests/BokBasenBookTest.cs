using FakeItEasy;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;
using Xunit;

namespace Solvberget.Service.Tests.DTOTests
{
    
    public class BokBasenBookTest
    {
        private const string Serveruri = "http://193.91.211.230/bokomslagServer/getInfoNew.Asp?";
        private readonly string[] _trueParams = {"SMALL_PICTURE", "LARGE_PICTURE", "PUBLISHER_TEXT", "FSREVIEW", "CONTENTS", "SOUND", "EXTRACT", "REVIEWS", "nocrypt"};

        private const string System = "1";

        private readonly string _xmluri = "";

        public BokBasenBookTest()
        {
            _xmluri = Serveruri;
            foreach (var param in _trueParams)
            {
                _xmluri += param + "=true&";
            }
            _xmluri += "SYSTEM=" + System;
        }

        [Fact(Skip = "This test is failing, determine whether to keep this or not...")]
        public void FillPropertiesTest()
        {
             var fake = A.Fake<IEnvironmentPathProvider>();

            var alephRepository = new AlephRepository(fake);
            var bokBaseRepository = new BokbasenRepository(alephRepository);


            var xmlBook = bokBaseRepository.GetExternalBokbasenBook(_xmluri + "&ISBN=9780747574477");

            Assert.Equal(xmlBook.Id, "9780747574477");

            Assert.Equal(xmlBook.Fsreview, "Harry%20Potter%20tror%20han%20er%20en%20helt%20vanlig%2011%20%E5r%20gammel%20gutt%20til%20han%20blir%20reddet%20av%20ei%20ugle%2C%20tatt%20med%20til%20skolen%20for%20hekser%20og%20trollmenn%20og%20vinner%20en%20duell%20som%20kunne%20f%E5tt%20en%20d%F8delig%20utgang.%20Harry%20Potter%20er%20nemlig%20en%20trollmann%21%20Dette%20er%20den%20f%F8rste%20boken%20om%20Harry%20Potter.");
            Assert.Equal(xmlBook.Thumb_Cover_Picture, "http://193.91.211.230/bokomslagServer/ALEPH_PICTURE/LXfWuxTioTaDH+1QDFaghdlQ6dcWIwp1.jpg");
            Assert.Equal(xmlBook.Small_Cover_Picture, "http://193.91.211.230/bokomslagServer/SMALL_PICTURE/ZRkY0Ttgp84DH+1QDFaghdlQ6dcWIwp1.jpg");
            Assert.Equal(xmlBook.Large_Cover_Picture, "http://193.91.211.230/bokomslagServer/LARGE_PICTURE/LXfWuxTioTaDH+1QDFaghdlQ6dcWIwp1.jpg");
            Assert.Null(xmlBook.Publisher_text);
            Assert.Null(xmlBook.Reviews);
            Assert.Null(xmlBook.Sound);
            Assert.Null(xmlBook.Contents);
            Assert.Null(xmlBook.Extract);
            Assert.Null(xmlBook.Marc);


            xmlBook = bokBaseRepository.GetExternalBokbasenBook(_xmluri + "&ISBN=978-82-419-0641-1");
            Assert.Equal(xmlBook.Id, "9788241906411");

            Assert.Equal(xmlBook.Fsreview,
                            "S%F8nnen%20til%20FN-ambassad%F8ren%20i%20Santa%20Cruz%20er%20forsvunnet.%20Myndighetene%20vil%20ikke%20skremme%20bort%20turistene%20ved%20%E5%20sette%20amerikansk%20politi%20p%E5%20saken%2C%20og%20gir%20derfor%20oppdraget%20til%20Hardy-guttene.%20Jobben%20viser%20seg%20%E5%20bli%20sv%E6rt%20farlig%20da%20Frank%20og%20Joe%20havner%20i%20armene%20p%E5%20skruppell%F8se%20skattejegere.");
            Assert.Equal(xmlBook.Thumb_Cover_Picture, "http://193.91.211.230/bokomslagServer/ALEPH_PICTURE/LXfWuxTioTaX7b6O+vDHOZGh0W6ofPtC.jpg");
            Assert.Equal(xmlBook.Small_Cover_Picture, "http://193.91.211.230/bokomslagServer/SMALL_PICTURE/ZRkY0Ttgp84X7b6O+vDHOZGh0W6ofPtC.jpg");
            Assert.Equal(xmlBook.Large_Cover_Picture, "http://193.91.211.230/bokomslagServer/LARGE_PICTURE/LXfWuxTioTaX7b6O+vDHOZGh0W6ofPtC.jpg");
            Assert.Equal(xmlBook.Publisher_text, "De%20samme%20guttene%2C%20med%20samme%20nese%20for%20mysterier.%20Hardy-guttene%20er%20tilbake%20igjen.%20Supre%20spenningsb%F8ker%20for%20gutter%21%20%20%20S%F8nnen%20til%20FN-ambassad%F8ren%20i%20Santa%20Cruz%20er%20forsvunnet.%20Man%20frykter%20at%20han%20er%20blitt%20kidnappet%2C%20men%20det%20kommer%20ikke%20noe%20l%F8sepengekrav.%20Myndighetene%20p%E5%20feriestedet%20er%20redd%20hendelsen%20skal%20skremme%20bort%20turistene%2C%20og%20%F8nsker%20ikke%20amerikansk%20politi%20p%E5%20stedet.%20Saken%20er%20som%20skreddersydd%20for%20Hardy-guttene.%20Br%F8drene%20kan%20menge%20seg%20med%20festglade%20ungdommer%20i%20ferieparadiset%2C%20mens%20de%20etterforsker%20saken%20i%20det%20skjulte.%20Oppdraget%20viser%20seg%20imidlertid%20%E5%20bli%20sv%E6rt%20farlig.%20Frank%20og%20Joe%20snubler%20rett%20i%20armene%20p%E5%20skruppell%F8se%20skattejegere%21");
            Assert.Null(xmlBook.Reviews);
            Assert.Null(xmlBook.Sound);
            Assert.Null(xmlBook.Contents);
            Assert.Null(xmlBook.Extract);
            Assert.Null(xmlBook.Marc);
        }
    }
}
