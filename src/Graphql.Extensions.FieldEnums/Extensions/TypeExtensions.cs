using System;

namespace Graphql.Extensions.FieldEnums.Types.Extensions
{
    public static class TypeExtensions
    {
        public static bool InheritsFromGenericType(this Type type, Type genericTypeDefinition)
        {
            return type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == genericTypeDefinition;
        }
    }
}
