using System.Linq;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    class LinticketRepositoryTests
    {
        private readonly IEventRepository _eventRepository;

        public LinticketRepositoryTests()
        {
            _eventRepository = new LinticketRepository();
        }

        [Fact]
        public void TestGetEvent()
        {
            var events = _eventRepository.GetEvents();
            var eventNr2544 = events.FirstOrDefault(x => x.Id.Equals("2544"));

            Assert.Equal("Lesestund", eventNr2544.Name);
            Assert.Equal("Barne- og ungdomsbiblioteket", eventNr2544.Location);
            Assert.Equal("7", eventNr2544.LocationId);
            Assert.Equal("2544", eventNr2544.Id);
            Assert.Equal("27. november 2012", eventNr2544.DateFormatted);
            Assert.Equal("November", eventNr2544.Month);
            Assert.Equal("http://www.linticket.no/program/S%F8lvberget/vis.php3?Arr=2544", eventNr2544.Link);
            Assert.Equal("11:00:00", eventNr2544.Start);
            Assert.Equal("11:30:00", eventNr2544.Stop);
            Assert.Equal("http://www.linticket.no/bilder/S%F8lvberget/arrangementer/00000002544/Lesestund.jpg", eventNr2544.PictureUrl);
            Assert.Equal("http://www.linticket.no/bilder/S%F8lvberget/arrangementer/00000002544/thmbLesestund.jpg.png", eventNr2544.ThumbUrl);
            Assert.Equal("For barn mellom 3 og 6 år, og voksne i alle aldre.<br /><br />", eventNr2544.Description);
            Assert.Equal("", eventNr2544.Teaser);
            Assert.Equal("5", eventNr2544.TypeId);
            Assert.Equal("Stavanger", eventNr2544.City);
            Assert.Equal("4006", eventNr2544.PostalCode);
            Assert.Equal("Sølvbergt 2", eventNr2544.Address);
            Assert.Equal("5.73361", eventNr2544.Longitude);
            Assert.Equal("58.9712", eventNr2544.Latitude);
            Assert.Equal("Barn", eventNr2544.TypeName);
        } 
    }
}
