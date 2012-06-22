using System;
using System.Collections.Generic;

namespace Solvberget.Domain.Abstract
{
    public interface ISpellingDictionary
    {
        List<String> Lookup(string value);
    }
}
