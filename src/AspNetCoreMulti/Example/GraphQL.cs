using System;
using GraphQL.Types;

namespace Example
{
    public class DogSchema : Schema
    {
        public DogSchema(IServiceProvider provider, DogQuery query)
            : base(provider)
        {
            Query = query;
        }
    }

    public class DogQuery : ObjectGraphType<object>
    {
        public DogQuery()
        {
            Field<StringGraphType>("say", resolve: context => "woof woof woof");
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

    public class CatQuery : ObjectGraphType<object>
    {
        public CatQuery()
        {
            Field<StringGraphType>("say", resolve: context => "meow meow meow");
        }
    }
}
