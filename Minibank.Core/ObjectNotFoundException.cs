using System;

namespace Minibank.Core
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string? message) : base(message)
        {
        }
    }
}
