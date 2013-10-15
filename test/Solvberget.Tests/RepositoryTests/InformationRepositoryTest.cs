using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{

    [TestFixture]
    internal class InformationRepositoryTest
    {

        private InformationRepositoryHtml _informationRepository;


        [SetUp]
        public void InitRepository()
        {

            _informationRepository = new InformationRepositoryHtml();

        }


        [Test]
        public void TestGetContactInformation()
        {
            Assert.NotNull(_informationRepository.GetContactInformation());
         

        }

        [Test]
        public void TestGetOpeningHoursInformation()
        {

            Assert.NotNull(_informationRepository.GetOpeningHoursInformation());
         

        }

    }
}
