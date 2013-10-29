using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
            return GetDocumentItemRulesFromFile(_folderPath+ItemRulesFile);
        }

        public static IEnumerable<ItemRule> GetDocumentItemRulesFromFile(string itemRulesFile)
        {
            var rules = new List<ItemRule>();
            if (File.Exists(itemRulesFile))
            {
                StreamReader file = null;
                try
                {
                    file = new StreamReader(itemRulesFile);
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        AddRuleToList(ref rules, line);
                    }
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }

            return rules;

        }
        private static void AddRuleToList(ref List<ItemRule> rules, string ruleLine)
        {
            var regex = new Regex(@"([^!]{5}) ([^!]{2}) ([^!]{2}) [^!]{1} ([^!]{30}) ([NY]{1}) ([NY]{1}) ([NYCT]{1}) [NYCT]{1} ([NY]{1}) [NYCT]{1} [NYCT]{1} [NYCT]{1} [NYCT]{1} \d{2} [NYCT]{1}");
            if (regex.Match(ruleLine).Success)
            {
                var itemRule = new ItemRule
                {
                    Library = regex.Match(ruleLine).Groups[1].ToString(),
                    ItemStatus = regex.Match(ruleLine).Groups[2].ToString(),
                    ProcessStatusCode = regex.Match(ruleLine).Groups[3].ToString(),
                    ProcessStatusText = regex.Match(ruleLine).Groups[4].ToString().Trim(),
                    CanBorrow = regex.Match(ruleLine).Groups[5].ToString().Equals("Y"),
                    CanRenew = regex.Match(ruleLine).Groups[6].ToString().Equals("Y"),
                    CanReserve = !regex.Match(ruleLine).Groups[7].ToString().Equals("N"),
                    ShownOnWebCataloge = regex.Match(ruleLine).Groups[8].ToString().Equals("Y")
                };

                rules.Add(itemRule);
            }
        }
    }
}
