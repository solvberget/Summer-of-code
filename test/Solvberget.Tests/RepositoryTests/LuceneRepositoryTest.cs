using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

using FakeItEasy;

using NUnit.Framework;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;


namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    internal class LuceneRepositoryTest
    {
        private LuceneRepository _repository;
        private string _basepath;
        private string _dictPath;
        private string _indexPath;

        [TestFixtureSetUp]
        public void InitRepository()
        {

            _basepath = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\bin\App_Data");
            _dictPath = Path.Combine(_basepath, @"ordlister\ord_test.txt");
            _indexPath = Path.Combine(_basepath, @"ordlister_index");

            var fake = A.Fake<IEnvironmentPathProvider>();

            A.CallTo(() => fake.GetDictionaryIndexPath()).Returns(_indexPath);
            A.CallTo(() => fake.GetSuggestionListPath()).Returns(_dictPath);

            var documentRepository = new AlephRepository(fake);
            _repository = new LuceneRepository(fake, documentRepository);

            DictionaryBuilder.Build(_dictPath, _indexPath);

        }
        [Test]
        public void TestCreateTargetFolder()
        {
            DictionaryBuilder.CreateTargetFolder(Path.Combine(_basepath, "testpath"));
            Assert.Pass();
        }


        [Test]
        public void TestBuildDictionary()
        {
            DictionaryBuilder.Build(_dictPath, _indexPath);
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
            const string testSolution = "fotball";

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
            Assert.Contains("Harry Potter", _repository.SuggestionList());
            Assert.Contains("villanden", _repository.SuggestionList());
        }


    }
}