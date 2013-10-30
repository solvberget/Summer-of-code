using System.Collections.Generic;

namespace Solvberget.Domain.Aleph
{
    public interface IRulesRepository
    {
        IEnumerable<ItemRule> GetItemRules();
    }
}