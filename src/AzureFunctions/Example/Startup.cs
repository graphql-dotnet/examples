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

            // Azure Functions is not using `Microsoft.Extensions.DependencyInjection`, instead it is using (DryIoc)[https://github.com/dadhi/DryIoc].
            // This results into another behavior if multiple Constructors exists and
            // for DocumentExecuter the one with one with no arguments should be called.
            // See also https://bitbucket.org/dadhi/dryioc/wiki/SelectConstructorOrFactoryMethod
            builder.Services.AddSingleton<IDocumentExecuter>(sp => new DocumentExecuter());
            builder.Services.AddGraphQL();
        }
    }
}
