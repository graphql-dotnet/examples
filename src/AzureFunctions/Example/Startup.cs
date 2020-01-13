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

            // Azure Functions do not use `Microsoft.Extensions.DependencyInjection`, instead they use
            // DryIoc - https://github.com/dadhi/DryIoc. This leads to a different behavior if multiple
            // constructors exists so for DocumentExecuter should be called one which has no arguments.
            // See also https://bitbucket.org/dadhi/dryioc/wiki/SelectConstructorOrFactoryMethod
            builder.Services.AddSingleton<IDocumentExecuter>(sp => new DocumentExecuter());
            builder.Services.AddGraphQL();
        }
    }
}
