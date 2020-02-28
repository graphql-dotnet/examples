using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Builders;
using GraphQL.Types;

namespace Graphql.Extensions.FieldEnums.Types.Extensions
{
    public static class MetaDataExtensions
    {
        public static FieldBuilder<TSourceType, TProperty> WithOriginalName<TSourceType, TProperty>
        (
            this FieldBuilder<TSourceType, TProperty> fieldBuilder,
            string originalName
        )
        {
            fieldBuilder.FieldType.Metadata[SharedConstants.OriginalPropertyName] = originalName;
            return fieldBuilder;
        }

        public static string GetOriginalName(this FieldType field)
        {
            if (field.Metadata.TryGetValue(SharedConstants.OriginalPropertyName, out var originalName))
            {
                return originalName.ToString();
            }

            return field.Name;
        }


        public static FieldBuilder<TSourceType, TProperty> WithSourceType<TSourceType, TProperty>
        (
            this FieldBuilder<TSourceType, TProperty> fieldBuilder,
            Type type = null
        )
        {
            fieldBuilder.FieldType.Metadata[SharedConstants.SourceType] = type ?? typeof(TSourceType);
            return fieldBuilder;
        }

        public static Type GetSourceType(this FieldType field)
        {
            if (field.Metadata.TryGetValue(SharedConstants.SourceType, out var sourceTypeRaw) && sourceTypeRaw is Type sourceType)
            {
                return sourceType;
            }

            return null;
        }
    }

    public static class GraphQLExtensions
    {

        /// <summary>
        /// Guesses the first type which is not a graphql lib type.
        /// WARNING! THIS METHOD IS NOT SAFE
        /// Its just for the lazy boyz
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type EnsureNoGraphQlCoreType(this Type type)
        {
            var assembly = typeof(ListGraphType).Assembly;
            if (type.Assembly == assembly)
            {
                return EnsureNoListGraphType(type.GetGenericArguments().First());
            }

            return type;
        }

        public static Type EnsureNoListGraphType(this Type type)
        {
            if (type.BaseType == typeof(ListGraphType))
            {
                return EnsureNoListGraphType(type.GetGenericArguments().First());
            }

            return type;
        }

        public static QueryArguments AddRange(this QueryArguments queryArguments, IEnumerable<QueryArgument> arguments)
        {
            foreach (var queryArgument in arguments)
            {
                queryArguments.Add(queryArgument);
            }

            return queryArguments;
        }

        public static FieldBuilder<TSourceType, TReturnType> SkipTakeOrderByArguments<TSourceType, TReturnType>
        (
            this FieldBuilder<TSourceType, TReturnType> source
        )
        {
            var guessedGraphType = source.FieldType.Type.EnsureNoGraphQlCoreType();
            var typedArg = typeof(TypeFieldEnumerationWithoutLists<>).MakeGenericType(guessedGraphType);

            source.FieldType.Arguments.AddRange(new[] {
                DefaultQueryArguments.Skip,
                DefaultQueryArguments.Take,
                DefaultQueryArguments.GetOrderBy(typedArg),
                DefaultQueryArguments.GetOrderByDesc(typedArg),
            });

            return source;
        }
    }
}
