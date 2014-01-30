using System.Collections.Generic;
using System.Linq;

namespace Solvberget.Core
{
    public static class EnumerableExtensions
    {
        public static List<T> NullSafeToList<T>(this IEnumerable<T> values)
        {
            return values == null ? new List<T>() : values.ToList();
        }
    }
}