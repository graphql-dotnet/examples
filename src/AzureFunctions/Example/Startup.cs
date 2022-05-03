using GraphQL;
using GraphQL.Caching;
using GraphQL.DI;
using GraphQL.Execution;
using GraphQL.MicrosoftDI;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StarWars;
using StarWars.Types;
using System;
using System.Collections.Generic;

[assembly: FunctionsStartup(typeof(Example.Startup))]

namespace Example
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<StarWarsData>();

            // Azure Functions do not use `Microsoft.Extensions.DependencyInjection`, instead they use
            // DryIoc - https://github.com/dadhi/DryIoc. This leads to a different behavior if multiple
            // constructors exists so for DocumentExecuter should be called one which has all arguments.
            // See also https://bitbucket.org/dadhi/dryioc/wiki/SelectConstructorOrFactoryMethod
            builder.Services.AddSingleton<IDocumentExecuter>(sp => new DocumentExecuter(
                sp.GetRequiredService<IDocumentBuilder>(),
                sp.GetRequiredService<IDocumentValidator>(),
                sp.GetRequiredService<IComplexityAnalyzer>(),
                sp.GetRequiredService<IDocumentCache>(),
                sp.GetService<IEnumerable<IConfigureExecutionOptions>>() ?? Array.Empty<IConfigureExecutionOptions>(),
                sp.GetRequiredService<IExecutionStrategySelector>()));

            builder.Services.AddGraphQL(b => b
                .AddSchema<StarWarsSchema>()
                .AddGraphTypes(typeof(StarWarsSchema).Assembly));
        }
    }
}
