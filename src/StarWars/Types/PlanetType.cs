using GraphQL.Types;

namespace StarWars.Types
{
    public class PlanetType : ObjectGraphType<Planet>
    {
        public PlanetType(StarWarsData data)
        {
            Name = "Planet";
            Description = "A planet in the Star Wars universe.";

            Field(d => d.Id).Description("The id of the planet.");
            Field(d => d.Name, nullable: true).Description("The name of the dplanetroid.");

            Field<ListGraphType<CharacterInterface>>(
                "mostFamousSith",
                resolve: context => data.GetHumanByIdAsync(context.Source.MostFamousSith)
            );
            Field<ListGraphType<CharacterInterface>>(
                "mostFamousJedi",
                resolve: context => data.GetHumanByIdAsync(context.Source.MostFamousJedi)
            );
        }
    }
}
