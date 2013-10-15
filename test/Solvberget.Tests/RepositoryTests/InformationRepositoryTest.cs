using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    internal class InformationRepositoryTest
    {
        private readonly InformationRepositoryHtml _informationRepository;

        public InformationRepositoryTest()
        {
            _informationRepository = new InformationRepositoryHtml();
        }

        [Fact]
        public void TestGetContactInformation()
        {
            Assert.NotNull(_informationRepository.GetContactInformation());
        }

        [Fact]
        public void TestGetOpeningHoursInformation()
        {
            Assert.NotNull(_informationRepository.GetOpeningHoursInformation());
        }
    }
}
