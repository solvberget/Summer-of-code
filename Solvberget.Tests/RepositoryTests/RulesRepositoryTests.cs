using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    [TestFixture]
    class RulesRepositoryTests
    {

        private RulesRepository _rulesRepository;
        private const string PathToRulesFolder = @"..\..\..\Solvberget.Service\bin\App_Data\rules\";

        [SetUp]
        public void InitRepository()
        { 
            _rulesRepository = new RulesRepository(PathToRulesFolder);
        }

        [Test]
        public void TestGetRules()
        {

            var rules = _rulesRepository.GetItemRules().ToList();
            Assert.NotNull(rules);

            // There should be 84 rules in total
            Assert.AreEqual(84, rules.Count());

            // The second rule (picked at random)
            Assert.AreEqual("NOR50", rules.ElementAt(1).Library);
            Assert.AreEqual("LO", rules.ElementAt(1).ProcessStatusCode);
            Assert.AreEqual("##", rules.ElementAt(1).ItemStatus);

            Assert.AreEqual("Tapt eks.", rules.ElementAt(1).ProcessStatusText);

            Assert.IsFalse(rules.ElementAt(1).CanBorrow);
            Assert.IsFalse(rules.ElementAt(1).CanRenew);
            Assert.IsFalse(rules.ElementAt(1).CanReserve);
            Assert.IsFalse(rules.ElementAt(1).ShownOnWebCataloge);

            // ItemStatusCode 04-rule
            //NOR50 04 ## L 4 uker                         Y Y Y N Y N N N N 00 N

            var itemStatus04Rule = rules.FirstOrDefault(x => x.ItemStatus.Equals("04"));
            Assert.NotNull(itemStatus04Rule);

            Assert.AreEqual("NOR50", itemStatus04Rule.Library);
            Assert.AreEqual("##", itemStatus04Rule.ProcessStatusCode);
            Assert.AreEqual("04", itemStatus04Rule.ItemStatus);

            Assert.AreEqual("4 uker", itemStatus04Rule.ProcessStatusText);

            Assert.IsTrue(itemStatus04Rule.CanBorrow);
            Assert.IsTrue(itemStatus04Rule.CanRenew);
            Assert.IsTrue(itemStatus04Rule.CanReserve);
            Assert.IsTrue(itemStatus04Rule.ShownOnWebCataloge);

        }

    }
}
