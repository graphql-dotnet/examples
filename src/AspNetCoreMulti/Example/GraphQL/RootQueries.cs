using GraphQL.Types;
using System.Collections.Generic;

namespace Example.GraphQL
{
    public class DogRootQuery : ObjectGraphType
    {
        public DogRootQuery(IEnumerable<IDogQuery> dogQueries)
        {
            foreach (var dogQuery in dogQueries)
            {
                dogQuery.AddQueryFields(this);
            }
        }
    }

    public class CatRootQuery : ObjectGraphType
    {
        public CatRootQuery(IEnumerable<ICatQuery> catQueries)
        {
            foreach (var catQuery in catQueries)
            {
                catQuery.AddQueryFields(this);
            }
        }
    }
}
