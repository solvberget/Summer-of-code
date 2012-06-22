using System.Collections.Generic;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface ISpellingDictionary
    {
        List<Document> Lookup(string value);
    }
}
