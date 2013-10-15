using System.Collections.Generic;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class RulesRepository : IRulesRepository
    {
        private const string StdFolderPath = @"App_Data\rules\";
        private const string ItemRulesFile = @"itemrules.txt";
        private readonly string _folderPath;

        public RulesRepository(string folderPath = null)
        {
            _folderPath = string.IsNullOrEmpty(folderPath) ? StdFolderPath : folderPath;
        }

        public IEnumerable<ItemRule> GetItemRules()
        {
            return ItemRule.GetDocumentItemRulesFromFile(_folderPath+ItemRulesFile);
        }
    }
}
