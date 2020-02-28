using GraphQL.Types;

namespace Graphql.Extensions.FieldEnums.Types
{
    public class FieldAwareQueryArgument<TType> : QueryArgument
    {
        public FieldAwareQueryArgument() : base(typeof(TypeFieldEnumerationWithoutLists<TType>))
        {
        }
    }
}