using System;
using System.Collections.Generic;
using System.Linq;
using Graphql.Extensions.FieldEnums.Types.Extensions;
using GraphQL.Types;

namespace Graphql.Extensions.FieldEnums.Types
{
    public class TypeFieldEnumerationWithoutLists<TType> : TypeFieldEnumeration<TType>
    {
        public TypeFieldEnumerationWithoutLists(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override IEnumerable<EnumValueDefinition> GetEnumValueDefinitions()
        {
            return base.GetEnumValueDefinitions().Where(x => ValidateEnumValueDefinition(x));
        }

        private bool ValidateEnumValueDefinition(EnumValueDefinition valueDefinition)
        {
            if (!(valueDefinition is FieldAwareEnumValueDefinition fieldDefinition))
            {
                return true;
            }
            if (typeof(ListGraphType).IsAssignableFrom(fieldDefinition.FieldType.Type))
            {
                return false;
            }
            else
            {
                var originalName = fieldDefinition.FieldType.GetOriginalName();
                var sourceType = fieldDefinition.FieldType.GetSourceType();
                if (sourceType == null)
                {
                    return true; //TODO or false, not sure, need more brain to evaluate
                }

                var requiredProperty = sourceType.GetProperty(originalName, comparisonType: StringComparison.OrdinalIgnoreCase);
                if (requiredProperty == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
