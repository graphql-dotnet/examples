using GraphQL;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StarWars;
using StarWars.Types;

[assembly: FunctionsStartup(typeof(Example.Startup))]

namespace Example
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<StarWarsData>();
            builder.Services.AddSingleton<StarWarsQuery>();
            builder.Services.AddSingleton<StarWarsMutation>();
            builder.Services.AddSingleton<HumanType>();
            builder.Services.AddSingleton<HumanInputType>();
            builder.Services.AddSingleton<DroidType>();
            builder.Services.AddSingleton<CharacterInterface>();
            builder.Services.AddSingleton<EpisodeEnum>();
            builder.Services.AddSingleton<ISchema, StarWarsSchema>();

            builder.Services.AddSingleton<IDocumentExecuter>(sp => new DocumentExecuter());
            builder.Services.AddGraphQL();
        }
    }
}
