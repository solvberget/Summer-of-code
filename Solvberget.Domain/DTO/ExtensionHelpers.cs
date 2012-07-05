using System;
using System.Collections.Generic;

namespace Solvberget.Domain.DTO
{
    public static class ExtensionHelpers
    {
        public static IEnumerable<string> SplitByLength(this string str, int maxLength)
        {

            if (str == null)
                yield return string.Empty;

            for (int index = 0; index < str.Length; index += maxLength)
            {
                yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
            }
        }

    }
}