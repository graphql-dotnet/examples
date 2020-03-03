using System.Collections.Generic;
using System.Text;
using GraphQL;
using Graphql.Extensions.FieldEnums.Exceptions;
using GraphQL.Types;

namespace Graphql.Extensions.FieldEnums
{
    public class SkipTakeOrderByArgument
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string OrderBy { get; set; }
        public string OrderByDesc { get; set; }

        public SkipTakeOrderByArgument()
        {
        }

        public static SkipTakeOrderByArgument Parse<T>(IResolveFieldContext<T> context)
        {
            var result = new SkipTakeOrderByArgument
            {
                Skip = context.GetArgument<int?>("skip", null),
                Take = context.GetArgument<int?>("take", null),
                OrderBy = context.GetArgument<string>("orderBy", null),
                OrderByDesc = context.GetArgument<string>("orderByDesc", null),
            };

            var hasOrderBy = !string.IsNullOrEmpty(result.OrderBy);
            var hasOrderByDesc = !string.IsNullOrEmpty(result.OrderByDesc);

            if (hasOrderBy == true && hasOrderByDesc == true)
            {
                throw new AmbiguousFilterException($"Cannot order by {result.OrderBy} and {result.OrderByDesc} at the same time");
            }

            return result;
        }
    }
}
