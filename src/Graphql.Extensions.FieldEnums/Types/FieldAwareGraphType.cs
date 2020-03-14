using System;
using System.Linq.Expressions;
using GraphQL;
using GraphQL.Builders;
using Graphql.Extensions.FieldEnums.Types.Extensions;
using GraphQL.Types;

namespace Graphql.Extensions.FieldEnums.Types
{
    public class FieldAwareGraphType<TSourceType> : ObjectGraphType<TSourceType>
    {
        public override FieldBuilder<TSourceType, TProperty> Field<TProperty>
        (
            string name,
            Expression<Func<TSourceType, TProperty>> expression,
            bool nullable = false,
            Type type = null
        )
        {
            var result = base
                .Field(name, expression, nullable, type)
                .WithOriginalName(expression.NameOf())
                .WithSourceType(typeof(TSourceType));

            if (result.FieldType.Name.Contains("ID"))
            {
                result.FieldType.Name = result.FieldType.Name.Replace("ID", "Id");
            }

            return result;
        }
    }
}
