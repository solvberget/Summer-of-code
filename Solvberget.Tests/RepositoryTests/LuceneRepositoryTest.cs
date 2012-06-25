using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Solvberget.Domain.Implementation;
using Solvberget.Service.Infrastructure;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    class LuceneRepositoryTest
    {

        [Test]
        public void TestLookup()
        {
            var repository = new LuceneRepository(@"C:\Users\Capgemini\Documents\GitHub\Solvberget\Solvberget.Service\bin\App_Data\ordlister\ord_bm.txt",
                @"C:\Users\Capgemini\Documents\GitHub\Solvberget\Solvberget.Service\bin\App_Data\ordlister_index");
            
            repository.BuildDictionary();
            
            string testString = "omerfulg";
            string testSolution = "sommerfugl";
           
            var suggestions = repository.Lookup(testString);
            var solution = suggestions.FirstOrDefault(suggestion => suggestion == testSolution);
            Assert.AreEqual(testSolution, solution);


            testString = "tarktor";
            testSolution = "traktor";

            suggestions = repository.Lookup(testString);
            solution = suggestions.FirstOrDefault(suggestion => suggestion == testSolution);
            Assert.AreEqual(testSolution, solution);

            testString = "fotbal";
            testSolution = "fotball";

            suggestions = repository.Lookup(testString);
            solution = suggestions.FirstOrDefault(suggestion => suggestion == testSolution);
            Assert.AreEqual(testSolution, solution);




        }

    }
}