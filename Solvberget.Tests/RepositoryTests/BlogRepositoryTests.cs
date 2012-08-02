using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    class BlogRepositoryTests
    {

        private IBlogRepository _repository;
        private const string PathToBlogsFolder = @"..\..\..\Solvberget.Service\bin\App_Data\blogs\";

        [SetUp]
        public void InitRepository()
        {
            _repository = new BlogRepository(PathToBlogsFolder);
        }

        [Test]
        public void TestGetBlogs()
        {

            var blogs = _repository.GetBlogs();
            Assert.NotNull(blogs);
            Assert.IsTrue(blogs.Count > 0);

        }
        [Test]

        public void GetBlogWithEntriesTest()
        {

            var blogs = _repository.GetBlogs();
            for (var i = 0; i < blogs.Count(); i++)
            {
                var blog = _repository.GetBlogWithEntries(i);
                Assert.NotNull(blog.Entries);
                Assert.IsNotEmpty(blog.Entries);
            }
        }

    }
}
