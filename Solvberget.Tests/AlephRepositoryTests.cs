using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests
{
    [TestFixture]
    public class AlephRepositoryTests
    {

        [Test]
        public void TestFind()
        {
            var searchString = "Naiv super";
            var repository = new AlephRepository();
            var documents = repository.Search(searchString);

            Assert.AreEqual(12, documents.Count);
        }
    }
}
