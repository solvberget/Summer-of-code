using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Tests.Properties;
using Xunit;

namespace Solvberget.Core.Tests.Services
{
    public class NancyOpeningHoursServiceTests
    {
        private readonly IOpeningHoursService _service;

        public NancyOpeningHoursServiceTests()
        {
            _service = new OpeningHoursService(new StaticStringFakeDownloader(Resources.OpeningHoursSampleJson));
        }

        [Fact]
        public async Task Should_properly_deserialize_all_items_in_json()
        {
            var results = (await _service.GetOpeningHours()).ToList();

            results.Count.Should().Be(5);
        }

        [Fact]
        public async Task Should_properly_deserialize_title()
        {
            var results = (await _service.GetOpeningHours()).ToList();

            results.First().Title.Should().Be("Kulturhusets foajé");
        }
    }
}
