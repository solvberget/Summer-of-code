using FakeItEasy;

using Nancy.Testing;

using Should;

using Solvberget.Domain.Abstract;
using Solvberget.Nancy.Modules;

using Xunit;

namespace Solvberget.Nancy.Tests
{
    public class SuggestionModuleTests
    {
        private readonly ISuggestionDictionary _dictionary;

        private readonly Browser _browser;

        public SuggestionModuleTests()
        {
            _dictionary = A.Fake<ISuggestionDictionary>();
            _browser = new Browser(config =>
            {
                config.Module<SuggestionModule>();
                config.Dependency(_dictionary);
            });
        }

        [Fact]
        public void GetShouldFetchAllSuggestionsFromDictionary()
        {
            // Given
            A.CallTo(() => _dictionary.SuggestionList()).Returns(new[] { "Harry Potter", "Harry Hole" });

            // When
            var response = _browser.Get("/suggestions", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<string[]>().Length.ShouldEqual(2);
        }

        [Fact]
        public void LookupShouldGetSuggestionsFromDictionary()
        {
            // Given
            A.CallTo(() => _dictionary.Lookup("Harry Popper")).Returns("Harry Potter");

            // When
            var response = _browser.Get("/suggestions/lookup", with =>
            {
                with.Query("q", "Harry Popper");
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.AsString().ShouldEqual("Harry Potter");
        }
    }
}