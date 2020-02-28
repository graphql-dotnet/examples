using System;
using System.Collections.Generic;
using GraphQL.Types;

namespace Graphql.Extensions.FieldEnums.Types
{
    public static class DefaultQueryArguments
    {
        public static IEnumerable<QueryArgument> SkipTakeOrderByArguments<TTargetType>()
            => SkipTakeOrderByArguments(typeof(TTargetType));

        public static IEnumerable<QueryArgument> SkipTakeOrderByArguments(Type targetType)
        {
            yield return Skip;
            yield return Take;
            yield return GetOrderBy(targetType);
            yield return GetOrderByDesc(targetType);
        }

        public static QueryArgument Skip => new QueryArgument(typeof(IntGraphType))
        {
            Name = "skip",
            Description = "skip n entries",
        };

        public static QueryArgument Take => new QueryArgument(typeof(IntGraphType))
        {
            Name = "take",
            Description = "take n entries",
        };

        public static QueryArgument GetOrderBy(Type type)
        {
            var typedArg = type;
            if (type.IsGenericType == false || type.GetGenericTypeDefinition() != typeof(TypeFieldEnumerationWithoutLists<>))
            {
                typedArg = typeof(TypeFieldEnumerationWithoutLists<>).MakeGenericType(type);
            }

            return new QueryArgument(typedArg)
            {
                Name = "orderBy",
                Description = "order by",
            };
        }

        public static QueryArgument GetOrderByDesc(Type type)
        {
            var typedArg = type;
            if (type.IsGenericType == false || type.GetGenericTypeDefinition() != typeof(TypeFieldEnumerationWithoutLists<>))
            {
                typedArg = typeof(TypeFieldEnumerationWithoutLists<>).MakeGenericType(type);
            }

            return new QueryArgument(typedArg)
            {
                Name = "orderByDesc",
                Description = "order by desc",
            };
        }
    }
}