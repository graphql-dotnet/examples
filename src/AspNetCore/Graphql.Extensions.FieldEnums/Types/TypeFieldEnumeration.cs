using System;
using System.Collections.Generic;
using Graphql.Extensions.FieldEnums.Types.Extensions;
using GraphQL.Types;

namespace Graphql.Extensions.FieldEnums.Types
{
    public class TypeFieldEnumeration<TType> : EnumerationGraphType
    {
        private readonly IServiceProvider serviceProvider;

        public TypeFieldEnumeration(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.Name = $"{typeof(TType).Name}_Enumeration";

            // ReSharper disable once VirtualMemberCallInConstructor
            foreach (var enumValueDefinition in this.GetEnumValueDefinitions())
            {
                base.AddValue(enumValueDefinition);
            }
        }

        public virtual IEnumerable<EnumValueDefinition> GetEnumValueDefinitions()
        {
            if (typeof(IComplexGraphType).IsAssignableFrom(typeof(TType)))
            {
                var graphType = this.serviceProvider.GetService<TType>() as IComplexGraphType;
                var fields = graphType.Fields;

                foreach (var field in fields)
                {
                    yield return new FieldAwareEnumValueDefinition
                    {
                        Name = field.Name,
                        Description = field.Description,
                        Value = field.GetOriginalName(),
                        DeprecationReason = null,
                        FieldType = field
                    };
                }
            }
            else
            {
                var fields = typeof(TType).GetProperties();
                foreach (var field in fields)
                {
                    yield return new EnumValueDefinition
                    {
                        Name = field.Name,
                        Value = field.Name,
                        Description = field.Name
                    };
                }
            }
        }
    }
}