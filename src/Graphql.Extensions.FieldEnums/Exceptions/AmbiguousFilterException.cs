using System;

namespace Graphql.Extensions.FieldEnums.Exceptions
{
    [Serializable]
    public class AmbiguousFilterException : Exception
    {
        public AmbiguousFilterException(string message) : base(message)
        {

        }
    }
}
