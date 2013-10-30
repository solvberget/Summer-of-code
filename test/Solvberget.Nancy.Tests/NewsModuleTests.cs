using System.Collections.Generic;

using FakeItEasy;

using Nancy.Testing;

using Should;
using Solvberget.Domain.Info;
using Solvberget.Nancy.Modules;

using Xunit;

namespace Solvberget.Nancy.Tests
{
    public class NewsModuleTests
    {
        private readonly INewsRepository _repository;

        private readonly Browser _browser;

        public NewsModuleTests()
        {
            _repository = A.Fake<INewsRepository>();
            _browser = new Browser(config =>
            {
                config.Module<NewsModule>();
                config.Dependency(_repository);
            });
        }

        [Fact]
        public void GetShouldFetchNewsItemsFromRepository()
        {
            // Given
            A.CallTo(() => _repository.GetNewsItems(30)).Returns(new List<NewsItem>
            {
                new NewsItem
                {
                    Title = "Google Offers Android app gets a new look and simpler redemption process.",
                    DescriptionUnescaped = "Blah blah blah..."
                },
                new NewsItem
                {
                    Title = "T-Mobile launches trio of budget Android smartphones and an LTE hotspot.",
                    DescriptionUnescaped = "Blah blah blah..."
                }
            });

            // When
            var response = _browser.Get("/news", with =>
            {
                with.Query("limit", "30");
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<List<NewsItem>>().Count.ShouldEqual(2);
        }
    }
}