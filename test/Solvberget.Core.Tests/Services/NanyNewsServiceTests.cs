using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Tests.Properties;
using Xunit;

namespace Solvberget.Core.Tests.Services
{
    public class NanyNewsServiceTests
    {
        [Fact]
        public async Task Should_properly_deserialize_nancy_datetimeoffset_serialization_format()
        {
            INewsService service = new NewsService(new StaticStringFakeDownloader(Resources.NewsSampleJson));
            var results = (await service.GetNews()).ToList();

            results.Count().Should().Be(10);
            results.First().Published.Date.Should().Be(new DateTime(2013, 10, 29));
            results.Last().Published.Date.Should().Be(new DateTime(2013, 10, 14));
        }
    }
}
