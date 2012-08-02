using NUnit.Framework;
using Solvberget.Domain.DTO;


namespace Solvberget.Service.Tests.DTOTests
{
    [TestFixture]
    public class InformationTest
    {
        private ContactWebPage _contactWebPage = new ContactWebPage();
        private OpeningHoursWebPage _openingHoursWebPage = new OpeningHoursWebPage();
        [Test]
        public void GetHtmlTest()
        {

            var getContactHtml = _contactWebPage.GetHtml();
            var getOpeningHoursHtml = _contactWebPage.GetHtml();
            Assert.NotNull(getContactHtml);
            Assert.NotNull(getOpeningHoursHtml);

        }

        [Test]
        public void FillPropertiesTest()
        {

            var getContactHtml = _contactWebPage.GetHtml();
            var getOpeningHoursHtml = _openingHoursWebPage.GetHtml();

            Assert.NotNull(getContactHtml);
            Assert.NotNull(getOpeningHoursHtml);
            _contactWebPage.FillProperties();
            _openingHoursWebPage.FillProperties();

        }


    }
}
