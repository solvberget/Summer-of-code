using System.Collections.Generic;

using FakeItEasy;

using Nancy.Testing;

using Should;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Nancy.Modules;

using Xunit;

namespace Solvberget.Nancy.Tests
{
    public class BlogModuleTests
    {
        private readonly IBlogRepository _repository;

        private readonly Browser _browser;

        public BlogModuleTests()
        {
            _repository = A.Fake<IBlogRepository>();
            _browser = new Browser(config =>
            {
                config.Module<BlogModule>();
                config.Dependency(_repository);
            });
        }

        [Fact]
        public void GetShouldFetchAllBlogsFromRepository()
        {
            // Given
            A.CallTo(() => _repository.GetBlogs()).Returns(new List<Blog>
            {
                new Blog { Title = "Test Blog 1" },
                new Blog { Title = "Test Blog 2" }
            });

            // When
            var response = _browser.Get("/blogs", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<List<Blog>>().Count.ShouldEqual(2);
        }

        [Fact]
        public void GetWithIdShouldFetchSingleBlogFromRepository()
        {
            // Given
            A.CallTo(() => _repository.GetBlogWithEntries(1234)).Returns(new Blog { Title = "Test Blog" });

            // When
            var response = _browser.Get("/blogs/1234", with =>
            {
                with.Accept("application/json");
                with.HttpRequest();
            });

            // Then
            response.Body.DeserializeJson<Blog>().Title.ShouldEqual("Test Blog");
        }
    }
}