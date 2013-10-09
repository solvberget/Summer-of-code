using System.Collections.Generic;

using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IRulesRepository
    {
        IEnumerable<ItemRule> GetItemRules();
    }
}