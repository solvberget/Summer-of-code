using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    class LinticketRepositoryTests
    {

        private IEventRepository _eventRepository;

        [SetUp]
        public void InitRepository()
        {
            _eventRepository = new LinticketRepository();
        }

        [Test]
        public void TestGetEvent()
        {
            var events = _eventRepository.GetEvents();
            var eventNr2544 = events.FirstOrDefault(x => x.Id.Equals("2544"));

            Assert.AreEqual("Lesestund", eventNr2544.Name);
            Assert.AreEqual("Barne- og ungdomsbiblioteket", eventNr2544.Location);
            Assert.AreEqual("7", eventNr2544.LocationId);
            Assert.AreEqual("2544", eventNr2544.Id);
            Assert.AreEqual("27. november 2012", eventNr2544.DateFormatted);
            Assert.AreEqual("November", eventNr2544.Month);
            Assert.AreEqual("http://www.linticket.no/program/S%F8lvberget/vis.php3?Arr=2544", eventNr2544.Link);
            Assert.AreEqual("11:00:00", eventNr2544.Start);
            Assert.AreEqual("11:30:00", eventNr2544.Stop);
            Assert.AreEqual("http://www.linticket.no/bilder/S%F8lvberget/arrangementer/00000002544/Lesestund.jpg", eventNr2544.PictureUrl);
            Assert.AreEqual("http://www.linticket.no/bilder/S%F8lvberget/arrangementer/00000002544/thmbLesestund.jpg.png", eventNr2544.ThumbUrl);
            Assert.AreEqual("For barn mellom 3 og 6 år, og voksne i alle aldre.<br /><br />", eventNr2544.Description);
            Assert.AreEqual("", eventNr2544.Teaser);
            Assert.AreEqual("5", eventNr2544.TypeId);
            Assert.AreEqual("Stavanger", eventNr2544.City);
            Assert.AreEqual("4006", eventNr2544.PostalCode);
            Assert.AreEqual("Sølvbergt 2", eventNr2544.Address);
            Assert.AreEqual("5.73361", eventNr2544.Longitude);
            Assert.AreEqual("58.9712", eventNr2544.Latitude);
            Assert.AreEqual("Barn", eventNr2544.TypeName);
        } 
    }
}
