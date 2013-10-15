using System;
using System.IO;

using FakeItEasy;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    internal class LuceneRepositoryTest
    {
        private readonly LuceneRepository _repository;
        private readonly string _basepath;
        private readonly string _dictPath;
        private readonly string _indexPath;

        public LuceneRepositoryTest()
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

        [Fact]
        public void TestCreateTargetFolder()
        {
            DictionaryBuilder.CreateTargetFolder(Path.Combine(_basepath, "testpath"));
        }

        [Fact]
        public void TestBuildDictionary()
        {
            DictionaryBuilder.Build(_dictPath, _indexPath);
        }

        [Fact]
        public void TestLookupSingleWord()
        {

            string testString = "omerfulg";
            string testSolution = "sommerfugl";


            var solution = _repository.Lookup(testString);

            Assert.Equal(testSolution, solution);


            testString = "tarktor";
            testSolution = "traktor";

            solution = _repository.Lookup(testString);
            Assert.Equal(testSolution, solution);

            testString = "fotbal";
            testSolution = "fotball";

            solution = _repository.Lookup(testString);
            Assert.Equal(testSolution, solution);

            testString = "håår";
            testSolution = "hår";

            solution = _repository.Lookup(testString);
            Assert.Equal(testSolution, solution);

        }


        [Fact]
        public void TestLookupSingleWordNotLowercase()
        {


            const string testString = "Foball";
            const string testSolution = "fotball";

            var solution = _repository.Lookup(testString);
            Assert.Equal(testSolution, solution);

        }

        [Fact]
        public void TestLookupEmptyInput()
        {


            const string testString = "";
            const string testSolution = "";

            var solution = _repository.Lookup(testString);
            Assert.Equal(testSolution, solution);

        }

        [Fact]
        public void TestLookupBookTitles()
        {


            const string testString = "Arry Poter";
            const string testSolution = "Harry Potter";

            var solution = _repository.Lookup(testString);
            Assert.Equal(testSolution, solution);

        }

        

     

     
        [Fact]
        public void TestLookupMultipleWords()
        {

            var testString = "Mine flotte fotbalsko";
            var testSolution = "Mine flotte fotballsko";

            var solution = _repository.Lookup(testString);
            Assert.Equal(testSolution, solution);

            testString = "min stoore tarktor";
            testSolution = "min store traktor";

            solution = _repository.Lookup(testString);
            Assert.Equal(testSolution, solution);

        }

        [Fact]
        public void TestSuggestionList()
        {
            Assert.Contains("Harry Potter", _repository.SuggestionList());
            Assert.Contains("villanden", _repository.SuggestionList());
        }


    }
}