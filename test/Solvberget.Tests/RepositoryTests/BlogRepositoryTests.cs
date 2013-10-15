using System.Linq;

using FakeItEasy;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    
    class BlogRepositoryTests
    {

        private IBlogRepository _repository;
        private const string PathToBlogsFolder = @"..\..\..\Solvberget.Service\bin\App_Data\blogs\";

        public BlogRepositoryTests()
        {
            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetBlogFeedPath()).Returns(PathToBlogsFolder);

            _repository = new BlogRepository(fake);
        }

        [Fact]
        public void TestGetBlogs()
        {

            var blogs = _repository.GetBlogs();
            Assert.NotNull(blogs);
            Assert.True(blogs.Count > 0);

        }
        [Fact]

        public void GetBlogWithEntriesTest()
        {

            var blogs = _repository.GetBlogs();
            for (var i = 0; i < blogs.Count(); i++)
            {
                var blog = _repository.GetBlogWithEntries(i);
                Assert.NotNull(blog.Entries);
                Assert.NotEmpty(blog.Entries);
            }
        }

    }
}
