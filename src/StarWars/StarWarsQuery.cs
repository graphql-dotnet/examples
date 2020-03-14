using System;
using Graphql.Extensions.FieldEnums;
using Graphql.Extensions.FieldEnums.Types;
using Graphql.Extensions.FieldEnums.Types.Extensions;
using GraphQL;
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
                ),
                resolve: context => data.GetHumanByIdAsync(context.GetArgument<string>("id")));


            Field<ListGraphType<HumanType>>(
                "humans",
                arguments: new QueryArguments(DefaultQueryArguments.SkipTakeOrderByArguments<HumanType>()),
                resolve: context => data.GetHumansAsync(SkipTakeOrderByArgument.Parse(context))
            );

            Func<IResolveFieldContext, string, object> func = (context, id) => data.GetDroidByIdAsync(id);

            FieldDelegate<DroidType>(
                "droid",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the droid" }
                ),

                resolve: func
            );
        }
    }
}
