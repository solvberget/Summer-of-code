using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using NUnit.Framework;
using Solvberget.Domain.Implementation;
using Solvberget.Service.Infrastructure;
using System.IO;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    class LuceneRepositoryTest
    {
        private LuceneRepository _repository;

        [TestFixtureSetUp]
        public void InitRepository()
        {

            string basepath = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\bin\App_Data");
            _repository = new LuceneRepository(Path.Combine(basepath, @"ordlister\ord_bm.txt"),
               Path.Combine(basepath, @"ordlister_index"),
               Path.Combine(basepath, @"ordlister\stopwords.txt"),
               Path.Combine(basepath, @"ordlister\ord_forslag.txt"));
 
        }

        [Test]
        public void TestBuildDictionary()
        {
            _repository.BuildDictionary();
            Assert.Pass();
        }

        [Test]
        public void TestLookupSingleWord()
        {
          
            string testString = "omerfulg";
            string testSolution = "sommerfugl";


            var solution = _repository.Lookup(testString);

            Assert.AreEqual(testSolution, solution);


            testString = "tarktor";
            testSolution = "traktor";

            solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

            testString = "fotbal";
            testSolution = "fotball";

            solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

            testString = "håår";
            testSolution = "hår";

            solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }


        [Test]
        public void TestLookupSingleWordNotLowercase()
        {


            const string testString = "Foball";
            const string testSolution = "Fotball";

            var solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }

        [Test]
        public void TestLookupEmptyInput()
        {


            const string testString = "";
            const string testSolution = "";

            var solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }

        [Test]
        public void TestLookupBookTitles()
        {


            const string testString = "Arry Poter";
            const string testSolution = "Harry Potter";

            var solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }

        [Test]
        public void TestLookupWithPeriod()
        {


            string testString = "Denne setningen avslutes med punktuum.";
            string testSolution = "Denne setningen avsluttes med punktum";

            var solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);
            
            testString = "Denne setningen. avslutes med punktuum.";
            testSolution = "Denne setningen avsluttes med punktum";

            solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }

        [Test]
        public void TestLookupXss()
        {


            string testString = "<script>alert('error')";
            string testSolution = "&lt;script&gt;alert(&apos;error&apos;)";

            var solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

            testString = "Mitt <script> <br> fjernes";
            solution = _repository.Lookup(testString);
            Assert.IsFalse(solution.Contains("<script>"));
            Assert.IsFalse(solution.Contains("<br>"));
         
        }

        [Test]
        public void TestLookupWordSplitError()
        {



             string testString = "Fotball sko";
             string testSolution = "Fotballsko";

             var solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

            testString = "Mine fotball sko er blå";
            testSolution = "Mine fotballsko er blå";

            solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);
        }

        [Test]
        public void TestLookupMultipleWords()
        {
            
            var testString = "Mine flotte fotbalsko";
            var testSolution = "Mine flotte fotballsko";

            var solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

            testString = "min stoore tarktor";
            testSolution = "min store traktor";

            solution = _repository.Lookup(testString);
            Assert.AreEqual(testSolution, solution);

        }

        [Test]
        public void TestSuggestionList()
        {
            Assert.Contains("harry potter",_repository.SuggestionList());
            Assert.Contains("villanden", _repository.SuggestionList());
        }
        [Test]
        public void TestAddWordToSuggestionString()
        {
           var suggestionString = "fotball er ";
           const string suggestionSolution = "fotball er gøy ";

           var newSuggestionString = _repository.AddWordToSuggestionString(ref suggestionString, "gøy");

           Assert.Equals( newSuggestionString, suggestionSolution );

        }
        
}