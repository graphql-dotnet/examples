using System;
using Graphql.Extensions.FieldEnums;
using Graphql.Extensions.FieldEnums.Types;
using Graphql.Extensions.FieldEnums.Types.Extensions;
using GraphQL.Types;
using StarWars.Types;

namespace StarWars
{
    public class StarWarsQuery : ObjectGraphType<object>
    {
        public StarWarsQuery(StarWarsData data)
        {
            Name = "Query";

            Field<CharacterInterface>("hero", resolve: context => data.GetDroidByIdAsync("3"));
            Field<HumanType>(
                "human",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the human" }
                ).AddRange(DefaultQueryArguments.SkipTakeOrderByArguments<HumanType>()),
                resolve: context =>
                {
                    var skipTakeArgs = SkipTakeOrderByArgument.Parse(context);

                    return data.GetHumanByIdAsync(context.GetArgument<string>("id"));
                });

            Func<ResolveFieldContext, string, object> func = (context, id) =>
            {
                var skipTakeArgs = SkipTakeOrderByArgument.Parse(context);
                return data.GetDroidByIdAsync(id);
            };

            FieldDelegate<DroidType>(
                "droid",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the droid" }
                ).AddRange(DefaultQueryArguments.SkipTakeOrderByArguments<DroidType>()),

                resolve: func
            );
        }
    }
}
