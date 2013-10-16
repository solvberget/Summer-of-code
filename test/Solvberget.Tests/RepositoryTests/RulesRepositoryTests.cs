using System.Linq;

using Solvberget.Domain.Implementation;

using Xunit;

namespace Solvberget.Service.Tests.RepositoryTests
{
    class RulesRepositoryTests
    {
        private readonly RulesRepository _rulesRepository;
        private const string PathToRulesFolder = @"..\..\..\..\src\Solvberget.Nancy\bin\Data\rules\";

        public RulesRepositoryTests()
        {
            _rulesRepository = new RulesRepository(PathToRulesFolder);
        }

        [Fact]
        public void TestGetRules()
        {

            var rules = _rulesRepository.GetItemRules().ToList();
            Assert.NotNull(rules);

            // There should be 84 rules in total
            Assert.Equal(84, rules.Count());

            // The second rule (picked at random)
            Assert.Equal("NOR50", rules.ElementAt(1).Library);
            Assert.Equal("LO", rules.ElementAt(1).ProcessStatusCode);
            Assert.Equal("##", rules.ElementAt(1).ItemStatus);

            Assert.Equal("Tapt eks.", rules.ElementAt(1).ProcessStatusText);

            Assert.False(rules.ElementAt(1).CanBorrow);
            Assert.False(rules.ElementAt(1).CanRenew);
            Assert.False(rules.ElementAt(1).CanReserve);
            Assert.False(rules.ElementAt(1).ShownOnWebCataloge);

            // ItemStatusCode 04-rule
            //NOR50 04 ## L 4 uker                         Y Y Y N Y N N N N 00 N

            var itemStatus04Rule = rules.FirstOrDefault(x => x.ItemStatus.Equals("04"));
            Assert.NotNull(itemStatus04Rule);

            Assert.Equal("NOR50", itemStatus04Rule.Library);
            Assert.Equal("##", itemStatus04Rule.ProcessStatusCode);
            Assert.Equal("04", itemStatus04Rule.ItemStatus);

            Assert.Equal("4 uker", itemStatus04Rule.ProcessStatusText);

            Assert.True(itemStatus04Rule.CanBorrow);
            Assert.True(itemStatus04Rule.CanRenew);
            Assert.True(itemStatus04Rule.CanReserve);
            Assert.True(itemStatus04Rule.ShownOnWebCataloge);
        }
    }
}
