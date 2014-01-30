using System.Collections.Generic;

using FakeItEasy;

using Nancy.Testing;

using Should;
using Solvberget.Domain.Events;
using Solvberget.Nancy.Modules;

using Xunit;

namespace Solvberget.Nancy.Tests
{
    public class EventModuleTests
    {
        private readonly IEventRepository _repository;

        private readonly Browser _browser;

        public EventModuleTests()
        {
            _repository = A.Fake<IEventRepository>();
            _browser = new Browser(config =>
            {
                config.Module<EventModule>();
                config.Dependency(_repository);
            });
        }

        [Fact(Skip="Events currently return hardcoded test data.")]
        public void GetShouldFetchAllEventsFromRepository()
        {
            // Given
            A.CallTo(() => _repository.GetEvents()).Returns(new List<Event>
            {
                new Event { Name = "Awesome Event 1", DateAsString = "10-10-2013" },
                new Event { Name = "Awesome Event 2", DateAsString = "08-12-2013" }
            });

            // When
            var response = _browser.Get("/events", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<List<Event>>().Count.ShouldEqual(2);
        }
    }
}