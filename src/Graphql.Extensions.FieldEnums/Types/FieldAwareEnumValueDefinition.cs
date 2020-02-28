using GraphQL.Types;

namespace Graphql.Extensions.FieldEnums.Types
{
    public class FieldAwareEnumValueDefinition : EnumValueDefinition
    {
        public FieldType FieldType { get; set; }
    }
}