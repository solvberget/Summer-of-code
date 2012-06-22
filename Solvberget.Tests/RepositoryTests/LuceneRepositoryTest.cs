using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    class LuceneRepositoryTest
    {

        [Test]
        public void TestLookup()
        {
            const string testString = "sommre";
            const string testSolution = "sommer";
            
            var repository = new LuceneRepository();

            var suggestions = repository.Lookup(testString);

            var solution = suggestions.Where(suggestion => suggestion == testSolution);

            Assert.AreEqual(testSolution, solution);
        }

    }
}
