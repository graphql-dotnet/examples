using System;
using System.Linq;
using System.Reflection;

namespace Graphql.Extensions.FieldEnums.Types.Extensions
{
    public static class PropertyInfoExtensions
    {
        private const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

        public static PropertyInfo GetProperty
        (
            this Type type,
            string name,
            BindingFlags bindingAttr = DefaultLookup,
            StringComparison comparisonType = StringComparison.Ordinal
        )
        {
            return type.GetProperties(bindingAttr).SingleOrDefault(x => string.Equals(name, x.Name, comparisonType));
        }

        public static PropertyInfo EnsureDeclaringPropertyInfo(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.DeclaringType == null || propertyInfo.ReflectedType == propertyInfo.DeclaringType)
            {
                return propertyInfo;
            }

            return propertyInfo.DeclaringType.GetProperty(propertyInfo.Name, DefaultLookup);
        }
    }
}
