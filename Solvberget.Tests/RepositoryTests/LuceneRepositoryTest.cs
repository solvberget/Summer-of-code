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
        public void TestLookupSingleWord()
        {

            var repository = new LuceneRepository(@"C:\Users\Capgemini\Documents\GitHub\Solvberget\Solvberget.Service\bin\App_Data\ordlister\ord_bm.txt",
                @"C:\Users\Capgemini\Documents\GitHub\Solvberget\Solvberget.Service\bin\App_Data\ordlister_index");
            repository.BuildDictionary();
            string testString = "omerfulg";
            string testSolution = "sommerfugl";


            var solution = repository.Lookup(testString);

            Assert.AreEqual(testSolution, solution);


            testString = "tarktor";
            testSolution = "traktor";

            solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

            testString = "fotbal";
            testSolution = "fotball";

            solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

            testString = "håår";
            testSolution = "hår";

            solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }


        [Test]
        public void TestLookupSingleWordNotLowercase()
        {

            var repository = new LuceneRepository();

            const string testString = "Foball";
            const string testSolution = "Fotball";

            var solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }

        [Test]
        public void TestLookupEmptyInput()
        {

            var repository = new LuceneRepository();

            const string testString = "";
            const string testSolution = "";

            var solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }

        [Test]
        public void TestLookupBookTitles()
        {

            var repository = new LuceneRepository();

            const string testString = "Arry Poter";
            const string testSolution = "Harry Potter";

            var solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }

        [Test]
        public void TestLookupWithPeriod()
        {

            var repository = new LuceneRepository();

            string testString = "Denne setningen avslutes med punktuum.";
            string testSolution = "Denne setningen avsluttes med punktum";

            var solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);
            
            testString = "Denne setningen. avslutes med punktuum.";
            testSolution = "Denne setningen avsluttes med punktum";

            solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }

        [Test]
        public void TestLookupXss()
        {

            var repository = new LuceneRepository();

            string testString = "<script>alert('error')";
            string testSolution = "&lt;script&gt;alert(&apos;error&apos;)";

            var solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

            testString = "Mitt <script> <br> fjernes";
            solution = repository.Lookup(testString);
            Assert.IsFalse(solution.Contains("<script>"));
            Assert.IsFalse(solution.Contains("<br>"));
         
        }

        [Test]
        public void TestLookupWordSplitError()
        {

          
            var repository = new LuceneRepository();
            repository.BuildDictionary();
             string testString = "Fotball sko";
             string testSolution = "Fotballsko";

            var solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

            testString = "Mine fotball sko er blå";
            testSolution = "Mine fotballsko er blå";

            solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);
        }

        [Test]
        public void TestLookupMultipleWords()
        {
            var repository = new LuceneRepository();
            
            var testString = "Mine flotte fotbalsko";
            var testSolution = "Mine flotte fotballsko";

            var solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

            testString = "min stoore tarktor";
            testSolution = "min store traktor";

            solution = repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }
    }
}