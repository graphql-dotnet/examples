using GraphQL.Types;
using System;
using static Example.GraphQL.Queries;

namespace Example.GraphQL
{
    public class DogSchema : Schema
    {
        public DogSchema(IServiceProvider provider, DogQuery query)
            : base(provider)
        {
            Query = query;
        }
    }

    public class CatSchema : Schema
    {
        public CatSchema(IServiceProvider provider, CatQuery query)
            : base(provider)
        {
            Query = query;
        }
    }
}
