using System.Linq;
using NUnit.Framework;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    public class AlephRepositoryTests
    {

        [Test]
        public void TestFind()
        {
            const string searchString = "naiv super";
            var repository = new AlephRepository();
            var documents = repository.Search(searchString);

            var books = documents.Where(x => x.GetType().Name.Equals("Book"));

            Assert.AreEqual(7, books.Count());
            Assert.AreEqual(3, documents.Count(x => x.GetType().Name.Equals("Document")));
        }
    }
}
