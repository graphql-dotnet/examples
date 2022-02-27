using GraphQL.Instrumentation;
using GraphQL.Types;
using System;

namespace StarWars
{
    public class StarWarsSchema : Schema
    {
        public StarWarsSchema(IServiceProvider provider)
            : base(provider)
        {
            Query = (StarWarsQuery)provider.GetService(typeof(StarWarsQuery)) ?? throw new InvalidOperationException();
            Mutation = (StarWarsMutation)provider.GetService(typeof(StarWarsMutation)) ?? throw new InvalidOperationException();

            FieldMiddleware.Use(new InstrumentFieldsMiddleware());
        }
    }
}
