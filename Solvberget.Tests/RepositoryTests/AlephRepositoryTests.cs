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
            const string searchString = "Naiv super";
            var repository = new AlephRepository();
            var documents = repository.Search(searchString);

            Assert.AreEqual(12, documents.Count);
        }
    }
}
