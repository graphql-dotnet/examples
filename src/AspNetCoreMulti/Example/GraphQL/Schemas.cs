using GraphQL.Types;
using System;

namespace Example.GraphQL
{
    public class DogSchema : Schema
    {
        public DogSchema(IServiceProvider provider, DogRootQuery query)
            : base(provider)
        {
            Query = query;
        }
    }

    public class CatSchema : Schema
    {
        public CatSchema(IServiceProvider provider, CatRootQuery query, CatRootMutation mutation)
            : base(provider)
        {
            Query = query;
            Mutation = mutation;
        }
    }
}
