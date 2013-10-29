using System;

namespace Solvberget.Domain.Implementation
{
    public class AlephException : Exception
    {
        public AlephException(string message) : base(message)
        {}
    }
}