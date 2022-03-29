using System;

namespace Minibank.Core
{
    public class ValidationException : Exception
    {
        public ValidationException(string? message) : base(message)
        {
        }
    }
}
