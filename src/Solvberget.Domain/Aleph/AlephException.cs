using System;

namespace Solvberget.Domain.Aleph
{
    public class AlephException : Exception
    {
        public AlephException(string message) : base(message)
        {}
    }
}