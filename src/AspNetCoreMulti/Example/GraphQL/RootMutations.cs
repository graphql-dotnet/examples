using System.Collections.Generic;
using GraphQL.Types;

namespace Example.GraphQL;

public class CatRootMutation : ObjectGraphType
{
    public CatRootMutation(IEnumerable<ICatMutation> mutations)
    {
        foreach (var mutation in mutations)
        {
            mutation.AddMutationFields(this);
        }
    }
}
